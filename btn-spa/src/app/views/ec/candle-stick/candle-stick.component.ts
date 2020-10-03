import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { WorkerService } from 'src/app/_core/_service/worker.service';
import { Socket } from 'ngx-socket-io';
import * as pluginDataLabels from 'chartjs-plugin-datalabels';
import { Color } from 'ng2-charts';
import { environment } from '../../../../environments/environment';

@Component({
  selector: 'app-candle-stick',
  templateUrl: './candle-stick.component.html',
  styleUrls: ['./candle-stick.component.css'],
  encapsulation: ViewEncapsulation.None
})
export class CandleStickComponent implements OnInit {
  public barChartOptions = {
    responsive: true,
    // We use these empty structures as placeholders for dynamic theming.
    scales: { xAxes: [{}], yAxes: [{
      ticks: {
        beginAtZero: true,
        steps: 10,
        stepValue: 5,
        max: 100
      }
    }] },
    plugins: {
      datalabels: {
        anchor: 'end',
        align: 'end',
        font: {
          size: 18,
          weight: 'bold'
        },
      }
    }
  };
  public workerID: any;
  public barChartData = [
    { data: [0], label: 'TT' },
    { data: [0], label: 'Standard' },
    { data: [0], label: 'Avg' },
    { data: [0], label: 'Best' },
    { data: [0], label: 'Latest' }
  ];
  public barChartColors: Color[] = [
    { backgroundColor: '#808080' }, // tt
    { backgroundColor: '#36a2eb' }, // std
    { backgroundColor: '#4bc0c0' }, // avg
    { backgroundColor: '#ff3784' }, // best
    { backgroundColor: 'orange' } // lt
  ];
  public barChartLabels = ['Avarage & Best & Standard Time Of Station'];
  public barChartType = 'bar';
  public barChartLegend = true;
  public barChartPlugins = [pluginDataLabels];
  btnID: any;
  button: any;
  dataIoT: any;
  excelData: any;
  avg: any;
  best: any;
  standard: any;
  tt: any;
  latest: any;
  constructor(
    private socket: Socket,
    private workerService: WorkerService,
    private route: ActivatedRoute,
    private alertify: AlertifyService) { }
  // custom code end
  ngOnInit(): void {
    this.onRouteChange();
    this.socket.on('message', (btn) => {
      console.log('socket io----------------- ', btn);
      if (Number(this.btnID) === btn) {
        this.loadChart();
      }
    });

  }
   onRouteChange() {
    this.route.data.subscribe(data => {
      this.workerID = this.route.snapshot.params.workerID;
      this.btnID = this.route.snapshot.params.btnID;
      this.loadChart();
    });
  }
  exportExcel() {
    window.location.href = `${environment.apiUrlEC}Worker/ExcelExport/${this.workerID}/${this.btnID}`;
  }
  loadChart() {
    this.workerService.chart(this.workerID, this.btnID).subscribe((obj: any) => {
      if (obj.chartData.length > 0) {
        this.tt = Number(obj.chartData[0].data[0]);
        this.standard = Number(obj.chartData[1].data[0]);
        this.avg = Number(obj.chartData[2].data[0]);
        this.best = Number(obj.chartData[3].data[0]);
        this.latest = Number(obj.chartData[4].data[0]);
        this.barChartData = obj.chartData;
      }
      this.button = obj.button;
    }, error => this.alertify.error(error + '') );
  }
}
