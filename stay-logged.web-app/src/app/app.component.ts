import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { LogsService } from './services/logs.service';

export interface Log {
  machineName: string;
  logType: string;
  message: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = ['type', 'name', 'message'];
  dataSource: MatTableDataSource<Log>;

  constructor(private logsService: LogsService) { }

  ngOnInit(): void {
    this.logsService.getLogs().subscribe(logs => {
      this.dataSource = new MatTableDataSource(logs);
    })
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
