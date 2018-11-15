import { Routes } from '@angular/router';

import { HomeComponent } from './home/home.component';

export const PagesRoutes: Routes = [
  {
    path: '',
    children: [
      {
        path: 'home',
        component: HomeComponent,
        data: {
          title: 'Home',
          urls: [{ title: 'Home', url: '/pages' }]
        }
      },
    ]
  },
];
