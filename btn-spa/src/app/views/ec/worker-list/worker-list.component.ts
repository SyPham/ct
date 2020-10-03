import { Component, OnInit, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { Socket } from 'ngx-socket-io';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { LineInfoService } from 'src/app/_core/_service/line.info.service';
import { WorkerService } from 'src/app/_core/_service/worker.service';

@Component({
  selector: 'app-worker-list',
  templateUrl: './worker-list.component.html',
  styleUrls: ['./worker-list.component.css']
})
export class WorkerListComponent implements OnInit {
  workers: any;
  lineID = 0;
  editSettings = { allowEditing: true, allowAdding: true, allowDeleting: true, newRowPosition: 'Normal' };
  filterSettings = { type: 'Excel' };
  toolbarOptions = ['Excel Export', 'Search'];
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  data: any;
  lineInfo: {
    id: number;
    po: string;
    batch: string;
    modelName: string;
    modelNO: string;
    articleNO: string;
    lineName: string;
  };
  lineName: any;
  constructor(
    private workerService: WorkerService,
    private lineInfoService: LineInfoService,
    private route: ActivatedRoute,
    private router: Router,
    private socket: Socket,
    private alertify: AlertifyService) { }
  ngOnInit(): void {
    this.lineInfo = {
      id: 0,
      po: '#N/A',
      batch: '#N/A',
      modelName: '#N/A',
      modelNO: '#N/A',
      articleNO: '#N/A',
      lineName: this.lineName
    };
    this.socket.on('message', (message) => {
      this.getWorkerByLine();
    });
    this.onRouteChange();
  }
  onSubmit() {
    // TODO: Use EventEmitter with form value
    console.warn(this.lineInfo);
    if (this.lineID === 0) {
      return;
    }
    if (this.lineInfo.id === 0) {
      this.createLineInfo();
    } else {
      this.updateLineInfo();
    }
  }
  actionBegin(args) {
    if (args.requestType === 'save' && args.action === 'edit') {

      if (args.previousData.operation !== args.data.operation) {
        const model = {
          workerID: args.data.workerID,
          operation: args.data.operation
        };
        this.workerService.updateOperation(model).subscribe(() => {
          this.getWorkerByLine();
        }, error => this.alertify.warning(error + ''));
      }

      if (args.previousData.taktTime !== args.data.taktTime) {
        const model = {
          buttonID: args.data.buttonID,
          taktTime: args.data.taktTime
        };
        this.workerService.updateTalkTime(model).subscribe(() => {
          this.getWorkerByLine();
        }, error => this.alertify.warning(error + ''));
      }

      if (args.previousData.standard !== args.data.standard) {
        const standardModel = {
          buttonID: args.data.buttonID,
          standard: args.data.standard
        };
        this.workerService.updateStandard(standardModel).subscribe(() => {
          this.getWorkerByLine();
        }, error => this.alertify.warning(error + ''));
      }
    }
  }

  toolbarClick(args): void {
    switch (args.item.text) {
      /* tslint:disable */
      case 'Excel Export':
        this.grid.excelExport();
        break;
      /* tslint:enable */
      case 'PDF Export':
        break;
    }
  }
  onRouteChange() {
    this.route.data.subscribe(data => {
      console.log('router', this.route.snapshot.params.lineName);
      this.lineName = this.route.snapshot.params.lineName;
      this.getLineInfoByLine();
      this.getWorkerByLine();
    });
  }
  async getWorkerByLine() {
    try {
      const obj = await this.workerService.getWorkersByLine(this.lineName).toPromise() as any;
      console.log('getWorkerByLine', obj);
      const dataIoT = obj.dataIoT;
      this.data = obj.data.map( (item: any) => {
        return {
          fullName: item.fullName,
          workerID : item.workerID,
          lineID : item.lineID,
          buttonCode: item.buttonCode,
          buttonID: item.buttonID,
          standard : item.standard,
          operation: item.operation,
          lineName: item.lineName,
          taktTime: +item.taktTime,
          avg: item.avg,
          best: item.best,
          latest: item.latest,
          frequency: item.frequency
        };
      });
      console.log('getWorkerByLine', this.data);

    } catch (error) {
      this.alertify.error(error + '');
    }
  }
  latest(dataIoT, item) {
    const data = dataIoT.filter(x => x.btnID === item.buttonCode);
    if (data.length === 0) { return 0; }
    const res = data.slice(data.length - 1, data.length);
    return +res[0].cycleTime || 0;
  }
  best(dataIoT, item) {
    const data = dataIoT.filter(x => x.btnID === item.buttonCode);
    if (data.length === 0) { return 0; }
    const cycleTimes = data.map(a => a.cycleTime);
    return +Math.min(...cycleTimes) || 0;
  }
  frequency(dataIoT, item) {
    const data = dataIoT.filter(x => x.btnID === item.buttonCode);
    if (data.length === 0) { return 0; }
    return data.length;
  }
  avg(dataIoT, item) {
    const data = dataIoT.filter(x => x.btnID === item.buttonCode);
    if (data.length > 0) {
      let total = 0;
      // tslint:disable-next-line:forin
      for (const key in data) {
        total += data[key].cycleTime;
      }
      const avg = total / data.length;
      return this.toFixedIfNecessary(avg, 1);
    }
    return 0;
  }
  toFixedIfNecessary(value, dp) {
    return +parseFloat(value).toFixed(dp);
  }
  routerLink(data) {
    const uri = `/ec/setting/line/worker-list/${this.lineName}/chart/${data.workerID}/${data.buttonCode}/${data.fullName}`;
    this.router.navigate([uri]);
  }
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }

  updateLineInfo() {
    this.lineInfoService.update(this.lineInfo).subscribe( () => {
      this.getLineInfoByLine();
      this.alertify.success(`Successfully!`);
    }, error => this.alertify.error(error + ''));
  }
  createLineInfo() {
    this.lineInfoService.create(this.lineInfo).subscribe(() => {
      this.getLineInfoByLine();
      this.alertify.success(`Successfully!`);
    }, error => this.alertify.error(error + ''));
  }
  getLineInfoByLine() {
    this.lineInfoService.getLineInfoByLine(this.lineName).subscribe((res: any) => {
      this.lineInfo = res;
    }, error => this.alertify.error(error + ''));
  }
}
