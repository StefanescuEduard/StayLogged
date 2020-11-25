import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval, Subscription } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { LogDto } from './dto/log.dto';
import { LogsService } from './services/logs.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['type', 'name', 'message'];
  dataSource: MatTableDataSource<LogDto>;
  logsIntervalSubscription: Subscription;

  constructor(private logsService: LogsService) {}

  ngOnInit(): void {
    this.logsIntervalSubscription = interval(5000)
      .pipe(
        mergeMap(() => {
          return this.logsService.getLogs();
        })
      )
      .subscribe(logs => {
        this.dataSource = new MatTableDataSource(logs);
      });
  }

  ngOnDestroy(): void {
    this.logsIntervalSubscription.unsubscribe();
  }

  applyFilter(event: Event): void {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
