import { SidebarItem } from './sidebar-item.model';

export interface SidebarSection {
  title?: string;
  items: SidebarItem[];
}
