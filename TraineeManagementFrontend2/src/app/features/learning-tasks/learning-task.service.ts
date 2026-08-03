import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { LearningTask } from '../../shared/models/learning-task';
import { CreateLearningTaskRequest } from '../../shared/models/create-learning-task-request';
import { UpdateLearningTaskRequest } from '../../shared/models/update-learning-task-request';

@Injectable({
  providedIn: 'root'
})
export class LearningTaskService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<LearningTask[]> {
    return this.http.get<LearningTask[]>(
      `${environment.apiBaseUrl}/learningtasks`
    );
  }

  getById(id: number): Observable<LearningTask> {
    return this.http.get<LearningTask>(
      `${environment.apiBaseUrl}/learningtasks/${id}`
    );
  }

  create(request: CreateLearningTaskRequest): Observable<LearningTask> {
    return this.http.post<LearningTask>(
      `${environment.apiBaseUrl}/learningtasks`,
      request
    );
  }

  update(id: number, request: UpdateLearningTaskRequest): Observable<any> {
    return this.http.put(
      `${environment.apiBaseUrl}/learningtasks/${id}`,
      request
    );
  }

  delete(id: number): Observable<any> {
    return this.http.delete(
      `${environment.apiBaseUrl}/learningtasks/${id}`
    );
  }

}