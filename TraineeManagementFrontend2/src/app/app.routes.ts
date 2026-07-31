import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { Layout } from './core/layout/layout/layout';
import { authGuard } from './core/guards/auth-guard';
import { TraineeList } from './features/trainees/trainee-list/trainee-list';
import { AddTrainee } from './features/trainees/add-trainee/add-trainee';
import { EditTrainee } from './features/trainees/edit-trainee/edit-trainee';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: '',
    component: Layout,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        component: Dashboard
      },{
        path: 'trainees',
        component: TraineeList
      },{
        path: 'trainees/add',
        component: AddTrainee,
      },{
        path: 'trainees/edit/:id',
        component: EditTrainee,
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];