import { Component, OnInit } from '@angular/core';
import { AlertifyService } from 'src/app/_core/_service/alertify.service';
import { BuildingService } from 'src/app/_core/_service/building.service';

@Component({
  selector: 'app-factory',
  templateUrl: './factory.component.html',
  styleUrls: ['./factory.component.css']
})
export class FactoryComponent implements OnInit {
  buildings: any;

  constructor(
    private buildingService: BuildingService,
    private alertify: AlertifyService) { }

  ngOnInit(): void {
    this.getTree();
  }
 async getTree() {
   try {
     this.buildings = await this.buildingService.getBuildingsAsTreeView().toPromise();
   } catch (error) {
     this.alertify.error(error + '');
   }
  }
}
