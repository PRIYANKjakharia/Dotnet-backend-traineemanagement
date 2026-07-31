import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, retryWhen } from 'rxjs';

import { environment } from '../../../environments/environment';

import { Trainee } from '../../shared/models/trainee';
import { PagedResponse } from '../../shared/models/paged-resopnse';
import { TraineeQuery } from '../../shared/models/trainee-query';
import { CreateTrainee } from '../../shared/models/create-trainee';

@Injectable({
  providedIn: 'root'
})
export class TraineeService {

  constructor(private http: HttpClient) {}

  getAll(query: TraineeQuery): Observable<PagedResponse<Trainee>> {

    const params = new HttpParams()
      .set('pageNumber', query.pageNumber)
      .set('pageSize', query.pageSize)
      .set('search', query.search)
      .set('status', query.status);

    return this.http.get<PagedResponse<Trainee>>(
      `${environment.apiBaseUrl}/trainees`,
      { params }
    );
  }
  create(request: CreateTrainee): Observable<Trainee> {
    return this.http.post<Trainee>(
      `${environment.apiBaseUrl}/trainees`,
      request
    );
  }
  getById(id: number): Observable<Trainee> {
    return this.http.get<Trainee>(
      `${environment.apiBaseUrl}/trainees/${id}`
    );
  }
  update(id: number , request: CreateTrainee): Observable<{message:string}> {
    return this.http.put<{message:string}>(
      `${environment.apiBaseUrl}/trainees/${id}`,
      request
    );
  }
}