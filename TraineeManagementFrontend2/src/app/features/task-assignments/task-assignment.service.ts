import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { TaskAssignment } from '../../shared/models/task-assignment';
import { CreateTaskAssignment } from '../../shared/models/create-task-assignment';
import { UpdateTaskAssignment } from '../../shared/models/update-task-assignment';

@Injectable({
  providedIn: 'root'
})
export class TaskAssignmentService {

  private apiUrl = `${environment.apiBaseUrl}/taskAssignments`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<TaskAssignment[]> {
    return this.http.get<TaskAssignment[]>(this.apiUrl);
  }

  getById(id: number): Observable<TaskAssignment> {
    return this.http.get<TaskAssignment>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateTaskAssignment): Observable<TaskAssignment> {
    return this.http.post<TaskAssignment>(this.apiUrl, request);
  }

  update(id: number, request: UpdateTaskAssignment): Observable<any> {
    return this.http.put(`${this.apiUrl}/${id}`, request);
  }

  delete(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/${id}`);
  }

}