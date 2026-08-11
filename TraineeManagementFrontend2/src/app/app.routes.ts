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
import { LearningTaskList } from './features/learning-tasks/learning-task-list/learning-task-list';
import { AddLearningTask } from './features/learning-tasks/add-learning-task/add-learning-task';
import { EditLearningTask } from './features/learning-tasks/edit-learning-task/edit-learning-task';
import { TaskAssignmentList } from './features/task-assignments/task-assignment-list/task-assignment-list';
import { EditTaskAssignment } from './features/task-assignments/edit-task-assignment/edit-task-assignment';
import { AddTaskAssignment } from './features/task-assignments/add-task-assignment/add-task-assignment';
import { UploadFile } from './features/submissions/upload-file/upload-file';
import { SubmissionList } from './features/submissions/submission-list/submission-list';
import { AddSubmission } from './features/submissions/add-submission/add-submission';
import { ReviewList } from './features/reviews/review-list/review-list';
import { AddReview } from './features/reviews/add-review/add-review';
import { CreateUser } from './features/users/create-user/create-user';

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
      },{
        path: 'mentors/edit/:id',
        component: EditMentor,
        canActivate: [authGuard]
      },{
        path: 'learningtasks',
        component: LearningTaskList,
        canActivate: [authGuard]
      },{
        path: 'learningtasks/add',
        component: AddLearningTask,
        canActivate: [authGuard]
      },{
        path: 'learningtasks/edit/:id',
        component: EditLearningTask,
        canActivate: [authGuard]
      },{
        path: 'taskassignments',
        component: TaskAssignmentList,
        canActivate: [authGuard]
      },{
        path: 'taskassignments/add',
        component: AddTaskAssignment,
        canActivate: [authGuard]
      },{
        path: 'taskassignments/edit/:id',
        component: EditTaskAssignment,
        canActivate: [authGuard]
      },{
        path: 'submissions',
        component: SubmissionList,
        canActivate: [authGuard]
      },{
        path: 'submissions/add',
        component: AddSubmission,
        canActivate: [authGuard]
      },{
        path: 'submissions/upload/:id',
        component: UploadFile,
        canActivate: [authGuard]
      },{
        path: 'reviews',
        component: ReviewList,
        canActivate: [authGuard]
      },{
        path: 'reviews/add',
        component: AddReview,
        canActivate: [authGuard]
      },{
        path: 'users/create',
        component: CreateUser,
        canActivate: [authGuard]
      },
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];