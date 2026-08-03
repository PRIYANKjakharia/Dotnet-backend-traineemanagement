import { Routes } from '@angular/router';
import { Login } from './features/auth/login/login';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { Layout } from './core/layout/layout/layout';
import { authGuard } from './core/guards/auth-guard';
import { TraineeList } from './features/trainees/trainee-list/trainee-list';
import { AddTrainee } from './features/trainees/add-trainee/add-trainee';
import { EditTrainee } from './features/trainees/edit-trainee/edit-trainee';
import { MentorList } from './features/mentors/mentor-list/mentor-list';
import { AddMentor } from './features/mentors/add-mentor/add-mentor';
import { EditMentor } from './features/mentors/edit-mentor/edit-mentor';

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
      },{
        path: 'mentors',
        component: MentorList,
        canActivate: [authGuard]
      },{
        path: 'mentors/add',
        component: AddMentor,
        canActivate: [authGuard]
      },
      {
        path: 'mentors/edit/:id',
        component: EditMentor,
        canActivate: [authGuard]
      },
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];