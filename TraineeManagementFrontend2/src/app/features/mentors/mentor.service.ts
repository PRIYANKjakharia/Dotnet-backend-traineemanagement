import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { Mentor } from '../../shared/models/mentor';
import { CreateMentorRequest } from '../../shared/models/create-mentor-request';
import { UpdateMentorRequest } from '../../shared/models/update-mentor-request';

@Injectable({
  providedIn: 'root'
})
export class MentorService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<Mentor[]> {
    return this.http.get<Mentor[]>(
      `${environment.apiBaseUrl}/mentors`
    );
  }

  getById(id: number): Observable<Mentor> {
    return this.http.get<Mentor>(
      `${environment.apiBaseUrl}/mentors/${id}`
    );
  }

  create(request: CreateMentorRequest): Observable<Mentor> {
    return this.http.post<Mentor>(
      `${environment.apiBaseUrl}/mentors`,
      request
    );
  }

  update(id: number, request: UpdateMentorRequest): Observable<any> {
    return this.http.put(
      `${environment.apiBaseUrl}/mentors/${id}`,
      request
    );
  }

  // delete(id: number): Observable<any> {
  //   return this.http.delete(
  //     `${environment.apiBaseUrl}/mentors/${id}`
  //   );
  // }
}