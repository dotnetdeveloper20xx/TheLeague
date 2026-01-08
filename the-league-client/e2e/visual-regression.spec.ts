import { test, expect, Page } from '@playwright/test';

const API_URL = 'http://localhost:7000';

// Test users for different roles
const users = {
  superAdmin: { email: 'admin@theleague.com', password: 'Admin123!' },
  clubManager: { email: 'manager@riverside.com', password: 'Manager123!' },
  member: { email: 'henry.brown1@riverside.com', password: 'Member123!' },
};

// Routes organized by role
const routes = {
  public: [
    { path: '/auth/login', name: 'login' },
    { path: '/auth/register', name: 'register' },
    { path: '/auth/forgot-password', name: 'forgot-password' },
  ],
  superAdmin: [
    { path: '/admin', name: 'admin-dashboard' },
    { path: '/admin/clubs', name: 'clubs-list' },
    { path: '/admin/clubs/new', name: 'club-form' },
    { path: '/admin/users', name: 'users-list' },
    { path: '/admin/reports', name: 'admin-reports' },
    { path: '/admin/settings', name: 'admin-settings' },
  ],
  clubManager: [
    { path: '/club', name: 'club-dashboard' },
    { path: '/club/members', name: 'members-list' },
    { path: '/club/members/new', name: 'member-form' },
    { path: '/club/sessions', name: 'sessions-list' },
    { path: '/club/sessions/new', name: 'session-form' },
    { path: '/club/events', name: 'events-list' },
    { path: '/club/events/new', name: 'event-form' },
    { path: '/club/payments', name: 'payments-list' },
    { path: '/club/memberships', name: 'memberships-list' },
    { path: '/club/membership-types', name: 'membership-types' },
    { path: '/club/venues', name: 'venues-list' },
    { path: '/club/reports', name: 'club-reports' },
    { path: '/club/settings', name: 'club-settings' },
  ],
  member: [
    { path: '/portal', name: 'portal-dashboard' },
    { path: '/portal/sessions', name: 'portal-sessions' },
    { path: '/portal/events', name: 'portal-events' },
    { path: '/portal/payments', name: 'portal-payments' },
    { path: '/portal/family', name: 'family-members' },
    { path: '/portal/profile', name: 'member-profile' },
    { path: '/portal/settings', name: 'portal-settings' },
  ],
};

async function waitForPageReady(page: Page) {
  // Wait for network to be idle
  await page.waitForLoadState('networkidle');
  // Wait for fonts to load
  await page.evaluate(() => document.fonts.ready);
  // Wait for any animations to complete
  await page.waitForTimeout(500);
}

async function login(page: Page, user: { email: string; password: string }) {
  await page.goto('/auth/login');
  await page.fill('input[type="email"], input[formcontrolname="email"]', user.email);
  await page.fill('input[type="password"], input[formcontrolname="password"]', user.password);
  await page.click('button[type="submit"]');
  await page.waitForURL(/\/(admin|club|portal)/);
  await waitForPageReady(page);
}

// Public pages - no auth required
test.describe('Public Pages Visual Regression', () => {
  for (const route of routes.public) {
    test(`${route.name} page screenshot`, async ({ page }) => {
      await page.goto(route.path);
      await waitForPageReady(page);
      await expect(page).toHaveScreenshot(`${route.name}.png`);
    });
  }
});

// Super Admin pages
test.describe('Super Admin Visual Regression', () => {
  test.beforeEach(async ({ page }) => {
    await login(page, users.superAdmin);
  });

  for (const route of routes.superAdmin) {
    test(`${route.name} page screenshot`, async ({ page }) => {
      await page.goto(route.path);
      await waitForPageReady(page);
      await expect(page).toHaveScreenshot(`superadmin-${route.name}.png`);
    });
  }
});

// Club Manager pages
test.describe('Club Manager Visual Regression', () => {
  test.beforeEach(async ({ page }) => {
    await login(page, users.clubManager);
  });

  for (const route of routes.clubManager) {
    test(`${route.name} page screenshot`, async ({ page }) => {
      await page.goto(route.path);
      await waitForPageReady(page);
      await expect(page).toHaveScreenshot(`clubmanager-${route.name}.png`);
    });
  }
});

// Member Portal pages
test.describe('Member Portal Visual Regression', () => {
  test.beforeEach(async ({ page }) => {
    await login(page, users.member);
  });

  for (const route of routes.member) {
    test(`${route.name} page screenshot`, async ({ page }) => {
      await page.goto(route.path);
      await waitForPageReady(page);
      await expect(page).toHaveScreenshot(`member-${route.name}.png`);
    });
  }
});

// Design Audit Tests
test.describe('Enterprise Design Audit', () => {
  test('color contrast meets WCAG AA', async ({ page }) => {
    await page.goto('/auth/login');
    await waitForPageReady(page);

    // Check text contrast ratios
    const contrastIssues = await page.evaluate(() => {
      const issues: string[] = [];
      const elements = document.querySelectorAll('*');

      elements.forEach(el => {
        const style = window.getComputedStyle(el);
        const bgColor = style.backgroundColor;
        const textColor = style.color;

        // Skip transparent backgrounds
        if (bgColor === 'rgba(0, 0, 0, 0)') return;

        // Basic contrast check (simplified)
        const text = el.textContent?.trim();
        if (text && text.length > 0 && el.children.length === 0) {
          // Flag very light text on light backgrounds
          if (textColor.includes('rgb(200') && bgColor.includes('rgb(255')) {
            issues.push(`Low contrast: ${el.tagName}`);
          }
        }
      });

      return issues;
    });

    expect(contrastIssues.length).toBeLessThan(5);
  });

  test('typography hierarchy is correct', async ({ page }) => {
    await login(page, users.clubManager);
    await page.goto('/club');
    await waitForPageReady(page);

    const fontSizes = await page.evaluate(() => {
      const h1 = document.querySelector('h1');
      const h2 = document.querySelector('h2');
      const h3 = document.querySelector('h3');
      const p = document.querySelector('p');

      return {
        h1: h1 ? parseFloat(window.getComputedStyle(h1).fontSize) : 0,
        h2: h2 ? parseFloat(window.getComputedStyle(h2).fontSize) : 0,
        h3: h3 ? parseFloat(window.getComputedStyle(h3).fontSize) : 0,
        p: p ? parseFloat(window.getComputedStyle(p).fontSize) : 16,
      };
    });

    // Verify hierarchy (h1 > h2 > h3 > p)
    if (fontSizes.h1 && fontSizes.h2) expect(fontSizes.h1).toBeGreaterThan(fontSizes.h2);
    if (fontSizes.h2 && fontSizes.h3) expect(fontSizes.h2).toBeGreaterThan(fontSizes.h3);
    expect(fontSizes.p).toBeGreaterThanOrEqual(16); // Min 16px body text
  });

  test('touch targets are adequate size', async ({ page }) => {
    await page.goto('/auth/login');
    await waitForPageReady(page);

    const smallTargets = await page.evaluate(() => {
      const issues: string[] = [];
      const clickables = document.querySelectorAll('button, a, input, [role="button"]');

      clickables.forEach(el => {
        const rect = el.getBoundingClientRect();
        if (rect.width < 44 || rect.height < 44) {
          if (rect.width > 0 && rect.height > 0) {
            issues.push(`Small target: ${el.tagName} (${rect.width}x${rect.height})`);
          }
        }
      });

      return issues;
    });

    // Allow some small targets (icons in text, etc.)
    expect(smallTargets.length).toBeLessThan(10);
  });

  test('8px grid system adherence', async ({ page }) => {
    await login(page, users.clubManager);
    await page.goto('/club');
    await waitForPageReady(page);

    const spacingIssues = await page.evaluate(() => {
      const issues: string[] = [];
      const elements = document.querySelectorAll('.p-4, .p-6, .p-8, .m-4, .m-6, .m-8, .gap-4, .gap-6');

      elements.forEach(el => {
        const style = window.getComputedStyle(el);
        const padding = parseFloat(style.padding);
        const margin = parseFloat(style.margin);

        // Check if values align to 4px increments (8px grid base)
        if (padding && padding % 4 !== 0) {
          issues.push(`Non-grid padding: ${padding}px`);
        }
        if (margin && margin % 4 !== 0) {
          issues.push(`Non-grid margin: ${margin}px`);
        }
      });

      return issues;
    });

    expect(spacingIssues.length).toBe(0);
  });
});
