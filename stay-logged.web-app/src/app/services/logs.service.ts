import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LogDto } from '../dto/log.dto';

@Injectable({
  providedIn: 'root',
})
export class LogsService {
  private readonly apiUrl: string;

  constructor(private http: HttpClient) {
    this.apiUrl = 'http://localhost:5000/logs';
  }

  getLogs(): Observable<LogDto[]> {
    return this.http.get<LogDto[]>(this.apiUrl);
  }
}
