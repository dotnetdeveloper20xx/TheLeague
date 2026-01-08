using Microsoft.EntityFrameworkCore;
using TheLeague.Api.DTOs;
using TheLeague.Api.Services.Interfaces;
using TheLeague.Core.Entities;
using TheLeague.Infrastructure.Data;

namespace TheLeague.Api.Services;

public class VenueService : IVenueService
{
    private readonly ApplicationDbContext _context;

    public VenueService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VenueDto>> GetVenuesAsync(Guid clubId)
    {
        var venues = await _context.Venues.IgnoreQueryFilters()
            .Where(v => v.ClubId == clubId && v.IsActive)
            .Include(v => v.Sessions)
            .Include(v => v.Events)
            .ToListAsync();

        return venues.Select(MapToDto);
    }

    public async Task<VenueDto?> GetVenueByIdAsync(Guid clubId, Guid id)
    {
        var venue = await _context.Venues.IgnoreQueryFilters()
            .Include(v => v.Sessions)
            .Include(v => v.Events)
            .FirstOrDefaultAsync(v => v.ClubId == clubId && v.Id == id);

        return venue == null ? null : MapToDto(venue);
    }

    public async Task<VenueDto> CreateVenueAsync(Guid clubId, VenueCreateRequest request)
    {
        var venue = new Venue
        {
            Id = Guid.NewGuid(),
            ClubId = clubId,
            Name = request.Name,
            Description = request.Description,
            Address = request.Address,
            PostCode = request.PostCode,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            Capacity = request.Capacity,
            Facilities = request.Facilities,
            ImageUrl = request.ImageUrl,
            IsPrimary = request.IsPrimary,
            IsActive = true
        };

        _context.Venues.Add(venue);
        await _context.SaveChangesAsync();

        return MapToDto(venue);
    }

    public async Task<VenueDto?> UpdateVenueAsync(Guid clubId, Guid id, VenueUpdateRequest request)
    {
        var venue = await _context.Venues.IgnoreQueryFilters()
            .Include(v => v.Sessions)
            .Include(v => v.Events)
            .FirstOrDefaultAsync(v => v.ClubId == clubId && v.Id == id);

        if (venue == null) return null;

        if (request.Name != null) venue.Name = request.Name;
        if (request.Description != null) venue.Description = request.Description;
        if (request.Address != null) venue.Address = request.Address;
        if (request.PostCode != null) venue.PostCode = request.PostCode;
        if (request.Latitude.HasValue) venue.Latitude = request.Latitude;
        if (request.Longitude.HasValue) venue.Longitude = request.Longitude;
        if (request.Capacity.HasValue) venue.Capacity = request.Capacity;
        if (request.Facilities != null) venue.Facilities = request.Facilities;
        if (request.ImageUrl != null) venue.ImageUrl = request.ImageUrl;
        if (request.IsActive.HasValue) venue.IsActive = request.IsActive.Value;
        if (request.IsPrimary.HasValue) venue.IsPrimary = request.IsPrimary.Value;

        await _context.SaveChangesAsync();
        return MapToDto(venue);
    }

    public async Task<bool> DeleteVenueAsync(Guid clubId, Guid id)
    {
        var venue = await _context.Venues.IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.ClubId == clubId && v.Id == id);

        if (venue == null) return false;

        venue.IsActive = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<VenueScheduleDto> GetVenueScheduleAsync(Guid clubId, Guid id, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var venue = await _context.Venues.IgnoreQueryFilters()
            .FirstOrDefaultAsync(v => v.ClubId == clubId && v.Id == id);

        if (venue == null) return new VenueScheduleDto(id, "Unknown", [], []);

        var from = fromDate ?? DateTime.UtcNow;
        var to = toDate ?? DateTime.UtcNow.AddMonths(1);

        var sessions = await _context.Sessions.IgnoreQueryFilters()
            .Where(s => s.ClubId == clubId && s.VenueId == id && s.StartTime >= from && s.StartTime <= to && !s.IsCancelled)
            .OrderBy(s => s.StartTime)
            .Select(s => new SessionListDto(
                s.Id, s.Title, s.Category, s.StartTime, s.EndTime, s.Capacity,
                s.CurrentBookings, s.Capacity - s.CurrentBookings, s.SessionFee, s.IsCancelled, venue.Name
            ))
            .ToListAsync();

        var events = await _context.Events.IgnoreQueryFilters()
            .Where(e => e.ClubId == clubId && e.VenueId == id && e.StartDateTime >= from && e.StartDateTime <= to && !e.IsCancelled)
            .OrderBy(e => e.StartDateTime)
            .Select(e => new EventListDto(
                e.Id, e.Title, e.Type, e.StartDateTime, e.EndDateTime, e.Capacity,
                e.CurrentAttendees, e.IsTicketed, e.TicketPrice, e.IsCancelled, e.IsPublished, e.ImageUrl
            ))
            .ToListAsync();

        return new VenueScheduleDto(id, venue.Name, sessions, events);
    }

    private static VenueDto MapToDto(Venue v) => new(
        v.Id,
        v.Name,
        v.Description,
        v.Address,
        v.PostCode,
        v.Latitude,
        v.Longitude,
        v.Capacity,
        v.Facilities,
        v.ImageUrl,
        v.IsActive,
        v.IsPrimary,
        v.Sessions?.Count ?? 0,
        v.Events?.Count ?? 0
    );
}
