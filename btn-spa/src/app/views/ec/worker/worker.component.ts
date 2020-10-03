import { Component, OnInit, ViewChild } from '@angular/core';
import { WorkerService } from 'src/app/_core/_service/worker.service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { EditService, ToolbarService, PageService, PageSettingsModel, ToolbarItems, GridComponent } from '@syncfusion/ej2-angular-grids';

@Component({
  selector: 'app-worker',
  templateUrl: './worker.component.html',
  styleUrls: ['./worker.component.css'],
  providers: [ToolbarService, EditService, PageService]
})
export class WorkerComponent implements OnInit {
  wokersData: object;
  buildings: [];
  fieldsBuilding: object = { text: 'name', value: 'id' };
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  buildingUsers: [];
  user: any;
  password = '';
  userID: number;
  buildingID: number;
  toolbar = ['Add', 'Edit', 'Delete', 'Update', 'Cancel', 'ExcelExport', 'Search'];
  passwordFake = `aRlG8BBHDYjrood3UqjzRl3FubHFI99nEPCahGtZl9jvkexwlJ`;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  @ViewChild('grid') public grid: GridComponent;
  worker: any;
  workerID: any;
  constructor(
    private workerService: WorkerService,
    private alertify: AlertifyService,
  ) { }

  ngOnInit() {
    this.getAllWorkers();
  }
  // life cycle ejs-gridpwo
  createdUsers() {
  }
  actionBegin(args) {
    if (args.requestType === 'save' && args.action === 'add') {
      this.worker = {
        id: 0,
        fullName: args.data.fullName,
        workerCode: args.data.workerCode,
        buildingID: this.buildingID,
        operation: args.data.operation
      };
      this.create();
    }
    if (args.requestType === 'save' && args.action === 'edit') {
      this.worker = {
        id: args.data.id,
        fullName: args.data.fullName,
        workerCode: args.data.workerCode,
        buildingID: this.buildingID,
        operation: args.data.operation
      };
      this.update();
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }
  toolbarClick(args) {
    switch (args.item.text) {
      case 'Excel Export':
        this.grid.excelExport({ hierarchyExportMode: 'All' });
        break;
      default:
        break;
    }
  }
  actionComplete(args) {
    if (args.requestType === 'edit') {
      (args.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
    }
    if (args.requestType === 'add') {
      (args.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
    }
  }
  dataBound() {
    document.querySelectorAll('button[aria-label=Update] > span.e-tbar-btn-text')[0].innerHTML = 'Save';
  }
  // end life cycle ejs-grid

  // api
  onChangeBuilding(args, data) {
    this.workerID = data.id;
    this.buildingID = args.itemData.id;
  }
  getLines() {
    return new Promise((res, rej) => {
      this.workerService.getLines().subscribe((result: any) => {
        this.buildings = result || [];
        res(result);
      }, error => {
        rej(error);
      });
    });
  }
  async getAllWorkers() {
    try {
      await this.getLines();
      this.workerService.getAllWorkers().subscribe( (res: any) => {
        const workers = res.map((item: any) => {
          return {
            id: item.id,
            fullName: item.fullName,
            workerCode: item.workerCode,
            building: item.building?.name,
            buildingID: item.buildingID,
            operation: item.operation
          };
        });
        this.wokersData = workers;
      });
    } catch (error) {
      this.alertify.error(error + '');
    }
  }

  getBuildingUsers() {
    return new Promise((res, rej) => {
      this.workerService.getBuildingUsers().subscribe(result => {
        this.buildingUsers = result as any;
        res(result);
      }, err => {
        rej(err);
      });
    });
  }

  delete(id) {
    this.workerService.delete(id).subscribe(res => {
      this.alertify.success('The worker has been deleted!');
      this.getAllWorkers();
    });
  }
  create() {
    this.workerService.create(this.worker).subscribe(() => {
      this.alertify.success('The worker has been created!');
      this.getAllWorkers();
      this.resetParameters();
    });
  }
  resetParameters() {
    this.worker = {
      id: 0,
      fullName: '',
      workerCode: '',
      buildingID: 0,
      operation: ''
    };
    this.buildingID = 0;
  }
  update() {
    this.workerService.update(this.worker).subscribe(res => {
      this.alertify.success('The worker has been updated!');
      this.getAllWorkers();
      this.resetParameters();
    });
  }
  // end api

  // template ejs-grid
  buildingTempate(userid): string {
    const buildingUser = this.buildingUsers.filter((item: any) => item.userID === userid) as any[];
    if (buildingUser.length === 0) {
      return '#N/A';
    }
    const buildingID = buildingUser[0].buildingID || 0;
    const building = this.buildings.filter((item: any) => item.id === buildingID) as any[];
    if (building.length === 0) {
      return '#N/A';
    }
    const buildingName = building[0].name;
    return buildingName || '#N/A';
  }
  // end template ejs-grid
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.pageSettings.pageSize + Number(index) + 1;
  }
}
