import { Injectable, signal } from '@angular/core';

export interface Notification {
  id: string;
  type: 'success' | 'error' | 'warning' | 'info';
  message: string;
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  private notifications = signal<Notification[]>([]);
  readonly notifications$ = this.notifications;

  private generateId(): string {
    return Math.random().toString(36).substring(2, 9);
  }

  success(message: string, duration = 5000): void {
    this.addNotification({ type: 'success', message, duration });
  }

  error(message: string, duration = 8000): void {
    this.addNotification({ type: 'error', message, duration });
  }

  warning(message: string, duration = 6000): void {
    this.addNotification({ type: 'warning', message, duration });
  }

  info(message: string, duration = 5000): void {
    this.addNotification({ type: 'info', message, duration });
  }

  private addNotification(notification: Omit<Notification, 'id'>): void {
    const id = this.generateId();
    const newNotification: Notification = { ...notification, id };

    this.notifications.update(current => [...current, newNotification]);

    if (notification.duration && notification.duration > 0) {
      setTimeout(() => this.remove(id), notification.duration);
    }
  }

  remove(id: string): void {
    this.notifications.update(current => current.filter(n => n.id !== id));
  }

  clear(): void {
    this.notifications.set([]);
  }
}
