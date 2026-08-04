import { Observable } from "rxjs/internal/Observable";
import { environment } from "../../../environments/environment";
import { Submission } from "../../shared/models/submission";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { CreateSubmission } from "../../shared/models/create-submission";

@Injectable({
    providedIn: 'root'
})
export class SubmissionService {

    private apiUrl = `${environment.apiBaseUrl}/submissions`;

    constructor(private http: HttpClient) {}

    getAll(): Observable<Submission[]> {
        return this.http.get<Submission[]>(this.apiUrl);
    }

    getById(id: number): Observable<Submission> {
        return this.http.get<Submission>(`${this.apiUrl}/${id}`);
    }

    create(request: CreateSubmission): Observable<Submission> {
        return this.http.post<Submission>(this.apiUrl, request);
    }
    uploadFile(submissionId: number, file: File) {
        const formData = new FormData();
        formData.append('file', file);
        return this.http.post(
            `${environment.apiBaseUrl}/submissions/${submissionId}/files`,
            formData
        );
    }

}