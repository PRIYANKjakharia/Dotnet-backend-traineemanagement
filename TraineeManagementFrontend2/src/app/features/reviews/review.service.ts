import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from '../../../environments/environment';

import { Review } from '../../shared/models/review';
import { CreateReview } from '../../shared/models/create-review';

@Injectable({
  providedIn: 'root'
})
export class ReviewService {

  private apiUrl = `${environment.apiBaseUrl}/reviews`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Review[]> {
    return this.http.get<Review[]>(this.apiUrl);
  }

  getById(id: number): Observable<Review> {
    return this.http.get<Review>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateReview): Observable<Review> {
    return this.http.post<Review>(this.apiUrl, request);
  }

}