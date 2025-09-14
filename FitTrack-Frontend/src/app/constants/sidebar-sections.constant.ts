import { SidebarSection } from '../models/sidebar-section.model';

export const SIDEBAR_SECTIONS: SidebarSection[] = [
  {
    items: [
      { label: 'Dashboard', icon: 'home', route: '/home/dashboard' },
      { label: 'Workouts', icon: 'fitness_center', route: '/home/workouts' },
      { label: 'Meals', icon: 'restaurant', route: '/home/meals' },
      { label: 'Weight', icon: 'monitor_weight', route: '/home/weight' },
    ],
  },
  {
    title: 'Search',
    items: [
      { label: 'Exercises', icon: 'directions_run', route: '/home/exercises' },
      { label: 'Foods', icon: 'local_dining', route: '/home/foods' },
      { label: 'Recipes', icon: 'menu_book', route: '/home/recipes' },
    ],
  },
  {
    title: 'Manage',
    items: [
      {
        label: 'Users',
        icon: 'manage_accounts',
        route: '/home/manage/users',
        permission: 'user:manage',
      },
      {
        label: 'Exercises',
        icon: 'directions_run',
        route: '/home/manage/exercises',
        permission: 'exercise:manage',
      },
      {
        label: 'Foods',
        icon: 'local_dining',
        route: '/home/manage/foods',
        permission: 'food:manage',
      },
      {
        label: 'Recipes',
        icon: 'menu_book',
        route: '/home/manage/recipes',
        permission: 'recipe:manage',
      },
    ],
  },
];
