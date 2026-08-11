import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

export interface CreateUserRequest {
  username: string;
  email: string;
  password: string;
  role: 'mentor' | 'trainee';
  firstName: string;
  lastName: string;
  techStack?: string;
  expertise?: string;
  status: 'Active' | 'Inactive';
}

export interface UserResponse {
  id: number;
  username: string;
  email: string;
  role: string;
  profileId: number;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {

  private readonly apiUrl = `${environment.apiBaseUrl}/users`;

  constructor(private http: HttpClient) {}

  create(request: CreateUserRequest): Observable<UserResponse> {
    return this.http.post<UserResponse>(
      this.apiUrl,
      request
    );
  }
}