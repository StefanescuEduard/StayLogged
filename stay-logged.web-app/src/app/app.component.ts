import { Component, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { LogDto } from './dto/log.dto';
import { LogsService } from './services/logs.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = ['type', 'name', 'message'];
  dataSource: MatTableDataSource<LogDto>;

  constructor(private logsService: LogsService) {}

  ngOnInit(): void {
    interval(5000)
      .pipe(
        mergeMap(() => {
          return this.logsService.getLogs();
        })
      )
      .subscribe(logs => {
        this.dataSource = new MatTableDataSource(logs);
      });
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
