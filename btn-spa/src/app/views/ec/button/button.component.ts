import { Component, OnInit, ViewChild } from '@angular/core';
import { ButtonService } from 'src/app/_core/_service/button.service';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { GridComponent } from '@syncfusion/ej2-angular-grids';
import { WorkerService } from 'src/app/_core/_service/worker.service';

@Component({
  selector: 'app-button',
  templateUrl: './button.component.html',
  styleUrls: ['./button.component.css']
})
export class ButtonComponent implements OnInit {
  button: any;
  data: any;
  editSettings = { showDeleteConfirmDialog: false, allowEditing: true, allowAdding: true, allowDeleting: true, mode: 'Normal' };
  toolbarOptions = ['ExcelExport', 'Add', 'Edit', 'Delete', 'Cancel', 'Search'];
  @ViewChild('grid') grid: GridComponent;
  pageSettings = { pageCount: 20, pageSizes: true, pageSize: 10 };
  filterSettings = { type: 'Excel' };
  users: any;
  userID = 0;
  workerID: any;
  constructor(
    private buttonService: ButtonService,
    private workerService: WorkerService,
    private alertify: AlertifyService,
  ) { }

  ngOnInit() {
    this.button = {
      id: 0,
      name: '',
      code: '',
      standard: 0
    };
    this.button = {
      id: 0,
      buttonID: '',
      workerID: 0,
      createdBy: JSON.parse(localStorage.getItem('user')).User.ID
    };
    this.getAllWorkers();
  }
  // api
  getAllButton() {
    this.buttonService.getAllButton().subscribe(res => {
      this.data = res;
    });
  }
  getAllButtonByWorkerID(workerID = 0) {
    const id = workerID || 0;
    this.buttonService.getAllButtonByWorkerID(id).subscribe(res => {
      this.data = res;
    });
  }
  getAllWorkers() {
    this.workerService.getAllWorkers().subscribe(res => {
      this.users = res;
    });
  }
  create() {
    this.buttonService.create(this.button).subscribe(() => {
      this.alertify.success('Add Button Successfully');
      this.getAllButton();
      this.resetButton();
    });
  }
  resetButton() {
    this.button = {
      id: 0,
      name: '',
      code: '',
      standard: 0,
      workerID: null
    };
  }
  update() {
    this.buttonService.update(this.button).subscribe(() => {
      this.alertify.success('Add Button Successfully');
      this.getAllButtonByWorkerID(this.workerID);
      this.resetButton();
    });
  }
  checkExistWorkerLinkButton() {
    return this.buttonService.checkExistWorkerLinkButton(this.workerID).toPromise();
  }
  unlinkWorkerWithButton() {
    return this.buttonService.unlinkWorkerWithButton(this.workerID).toPromise();
  }
  checkExistButtonLinkWorker(btn) {
    return this.buttonService.checkExistButtonLinkWorker(btn).toPromise();
  }
  unlinkButtonLinkWorker(btn) {
    return this.buttonService.unlinkButtonLinkWorker(btn).toPromise();
  }
  delete(id) {
    this.alertify.confirm('Delete Button', 'Are you sure you want to delete this Button "' + id + '" ?', () => {
      this.buttonService.delete(id).subscribe(() => {
        this.getAllButton();
        this.alertify.success('The button has been deleted');
      }, error => {
        this.alertify.error('Failed to delete the button');
      });
    });
  }
  // end api

  // grid event
 async mapping(args, data) {
    console.log('mapping', args);
    console.log('mapping', data);
    if (this.workerID === 0) {
      this.alertify.warning('Please select a user to mapping!!!');
      this.grid.refresh();
      return;
    }
    if (args.checked === true) {
      const checkExistWorker = await this.checkExistWorkerLinkButton() as any;
      const checkExistButton = await this.checkExistButtonLinkWorker(data.id) as any;

      if (checkExistWorker === true || checkExistButton === true) {
        this.alertify.confirm('Warning', 'This button linked with the other user! Do you want to unlink that user?', async () => {
          if (checkExistButton === true) {
            await this.unlinkButtonLinkWorker(data.id);
          }
          if (checkExistWorker === true) {
            await this.unlinkWorkerWithButton();
          }
          this.button = {
            id: data.id,
            standard: data.standard,
            name: data.name,
            code: '',
            workerID: this.workerID
          };
          console.log('unlinkWorkerWithButton', this.button);
          this.update();
        });
        this.grid.refresh();
        return;
      } else {
        this.button = {
          id: data.id,
          standard: data.standard,
          name: data.name,
          code: '',
          workerID: this.workerID
        };
        console.log('mapping add', this.button);
        this.update();
      }
    } else {
      this.button = {
        id: data.id,
        code: '',
        standard: data.standard,
        name: data.name,
        workerID: null
      };
      console.log('mapping update', args);
      this.update();
    }
  }
  toolbarClick(args): void {
    switch (args.item.text) {
      /* tslint:disable */
      case 'Excel Export':
        this.grid.excelExport();
        break;
      /* tslint:enable */
      default:
        break;
    }
  }
  rowSelected(args) {
    console.log(args);
    if (args.isInteracted) {
      this.workerID = args.data.id;
      this.getAllButtonByWorkerID(this.workerID);
    }
  }
  actionBegin(args) {
    if (args.requestType === 'save') {
      if (args.action === 'add') {
        this.button.id = 0;
        this.button.name = args.data.name;
        this.button.standard = args.data.standard;
        this.create();
      }
      if (args.action === 'edit') {
        this.button.id = args.data.id;
        this.button.standard = args.data.standard;
        this.update();
      }
    }
    if (args.requestType === 'delete') {
      this.delete(args.data[0].id);
    }
  }
  actionComplete(e: any): void {
    if (e.requestType === 'add') {
      (e.form.elements.namedItem('name') as HTMLInputElement).focus();
      (e.form.elements.namedItem('id') as HTMLInputElement).disabled = true;
      (e.form.elements.namedItem('mapping') as HTMLInputElement).disabled = true;
    }
  }
  // end event
  NO(index) {
    return (this.grid.pageSettings.currentPage - 1) * this.grid.pageSettings.pageSize + Number(index) + 1;
  }
}
