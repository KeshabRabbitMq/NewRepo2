import { Component, NgZone, ViewChild } from '@angular/core';
import { AppService } from './app.service';

import {
  ChartConfiguration,
  ChartDataset,
  ChartEvent,
  ChartOptions,
  ChartType,
} from 'chart.js';
import { BaseChartDirective } from 'ng2-charts';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Model } from './model';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  public newJsonData: any = [];
  public postData: any = JSON.stringify({
    Id: 'servicemonitoraggregate-7f171778-1287-4ec1-9152-6d7f5d7fe954',
  });

  public createData(d: any) {
    var data = [];
    for (var ii = 0; ii < d.length; ii += 1) {
      data.push(d[ii].value);
    }
    return data;
  }

  public createLabel(d: any) {
    var data = [];
    for (var ii = 0; ii < d.length; ii += 1) {
      data.push(
        new Date(d[ii].key).toLocaleString('en-US', { hour12: true }).split(' ')
      );
    }
    return data;
  }

  constructor(public signalRService: AppService, public http: HttpClient) {
    this.signalRService.startConnection();
    this.addTransferChartDataListener();
    this.pull();
  }

  ngOnInit() {}

  public addTransferChartDataListener = () => {
    this.signalRService.hubConnection.on('SendMethodLog', (data) => {
      if (data.length > 0) {
        if (this.newJsonData.length == 0) this.pull();
        else {
          var delta: any = [];
          var d = {
            key: JSON.parse(data)['ExecutionTime'],
            value: JSON.parse(data)['TimeElapsed'],
          };
          this.refresh(d);
        }
      }
    });
  };

  refresh(d:any){
    var data:any=[];
    data.push(d);
    this.lineChartData.datasets[0].data.push(d.value);
    this.lineChartData.labels.push(this.createLabel(data)[0]);
    this.chart?.update();
  }

  

  pull() {
    let headers = new HttpHeaders({
      'Content-Type': 'application/json',
    });
    let options = { headers: headers };

    this.http
      .post(
        'https://localhost:44329/ServiceMonitor/queryservice',
        this.postData,
        options
      )
      .subscribe((data: any) => {
        this.newJsonData = data;
        if (this.newJsonData.length > 0)
          this.refreshData();
      });
  }

  public lineChartData: any;

  refreshData() {
    this.lineChartData = {
      datasets: [
        {
          data: this.createData(this.newJsonData[0].data),
          label: this.newJsonData[0].name,
          yAxisID: 'y-axis-1',
          backgroundColor: 'lightblue',
          borderColor: 'blue',
          pointBackgroundColor: 'green',
          pointBorderColor: '#fff',
          pointHoverBackgroundColor: '#fff',
          pointHoverBorderColor: 'rgba(148,159,177,0.8)',
          fill: true,
        },
      ],
      labels: this.createLabel(this.newJsonData[0].data),
      responsive: true,
      maintainAspectRatio: false,
    };
  }

  public lineChartOptions: ChartConfiguration['options'] = {
    elements: {
      line: {
        tension: 0.5,
      },
    },
    scales: {
      // We use this empty structure as a placeholder for dynamic theming.
      x: {},
      'y-axis-1': {
        position: 'left',
        grid: {
          color: 'green',
        },
        ticks: {
          callback: function (val: any, index: any, label: any) {
            // Hide every 2nd tick label
            return val.toFixed(7) + '(s)';
          },
        },
      },
    },
  };

  public lineChartType: ChartType = 'line';
  public lineChartLegend = true;
  public lineChartPlugins = [];

  @ViewChild(BaseChartDirective) chart?: BaseChartDirective;

  // events
  public chartClicked({
    event,
    active,
  }: {
    event?: ChartEvent;
    active?: {}[];
  }): void {
    console.log(event, active);
  }

  public chartHovered({
    event,
    active,
  }: {
    event?: ChartEvent;
    active?: {}[];
  }): void {
    console.log(event, active);
  }
}
