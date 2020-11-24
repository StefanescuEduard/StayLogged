import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Log } from '../app.component';

@Injectable({
  providedIn: 'root',
})
export class LogsService {
  private readonly apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = 'http://localhost:5000/logs';
  }

  getLogs(): Observable<Log[]> {
    return this.http.get<Log[]>(this.apiUrl);
  }
}
