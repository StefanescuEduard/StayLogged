import { Component, OnDestroy, OnInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { interval, Subscription } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { ChartLogDto } from './dto/chart-log.dto';
import { LogDto } from './dto/log.dto';
import { LogsService } from './services/logs.service';

export class Area {
  country: string;
  area: number;
}

let areas: Area[] = [
  {
    country: 'Russia',
    area: 12,
  },
  {
    country: 'Canada',
    area: 7,
  },
  {
    country: 'USA',
    area: 7,
  },
  {
    country: 'China',
    area: 7,
  },
  {
    country: 'Brazil',
    area: 6,
  },
  {
    country: 'Australia',
    area: 5,
  },
  {
    country: 'India',
    area: 2,
  },
  {
    country: 'Others',
    area: 55,
  },
];

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = [
    'type',
    'dateTime',
    'source',
    'description',
    'ip',
  ];
  dataSource: MatTableDataSource<LogDto>;
  logsIntervalSubscription: Subscription;
  errors: ChartLogDto[];
  info: ChartLogDto[];

  constructor(private logsService: LogsService) {}

  ngOnInit(): void {
    this.logsIntervalSubscription = interval(5000)
      .pipe(
        mergeMap(() => {
          return this.logsService.getLogs();
        })
      )
      .subscribe((logs) => {
        this.dataSource = new MatTableDataSource(logs);
      });

    this.logsService.getChart('ERROR').subscribe((data) => {
      this.errors = data;
    });

    this.logsService.getChart('INFORMATION').subscribe((data) => {
      this.info = data;
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
