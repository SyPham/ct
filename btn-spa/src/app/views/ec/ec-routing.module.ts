
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { BuildingComponent } from './building/building.component';
import { AccountComponent } from './account/account.component';

import { ButtonComponent } from './button/button.component';
import { FactoryComponent } from './factory/factory.component';
import { WorkerComponent } from './worker/worker.component';
import { WorkerListComponent } from './worker-list/worker-list.component';
import { CandleStickComponent } from './candle-stick/candle-stick.component';

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'ec',
      breadcrumb: 'Home'
    },
    children: [

      // setting
      {
        path: 'setting',
        data: {
          title: 'setting',
          breadcrumb: 'Setting'
        },
        children: [
          {
            path: 'account-1',
            component: AccountComponent,
            data: {
              title: 'account',
              breadcrumb: 'Account'
            }
          },
          {
            path: 'building',
            component: BuildingComponent,
            data: {
              title: 'building',
              breadcrumb: 'Building'
            }
          },
          {
            path: 'worker',
            component: WorkerComponent,
            data: {
              title: 'Worker',
              breadcrumb: 'Worker'
            }
          },
          {
            path: 'line',
            data: {
              title: 'Line',
              breadcrumb: 'Line'
            },
            children: [
              {
                path: '',
                component: FactoryComponent,
              },
              {
                path: 'worker-list/:lineName',
                data: {
                  title: 'Worker list',
                  breadcrumb: 'Worker List',
                },
                children: [
                  {
                    path: '',
                    component: WorkerListComponent,
                  },
                  {
                    path: 'chart/:workerID/:btnID/:fullName',
                    component: CandleStickComponent,
                    data: {
                      title: 'chart',
                      breadcrumb: 'Chart'
                    }
                  }
                ]
              }
            ]
          },
          {
            path: 'button',
            component: ButtonComponent,
            data: {
              title: 'Button',
              breadcrumb: 'Button'
            }
          }
        ]
      },
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ECRoutingModule { }
