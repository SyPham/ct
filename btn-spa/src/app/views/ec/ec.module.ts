// Angular
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { NgxSpinnerModule } from 'ngx-spinner';
// Components Routing
import { ECRoutingModule } from './ec-routing.module';
import { NgSelectModule } from '@ng-select/ng-select';

import { DropDownListModule } from '@syncfusion/ej2-angular-dropdowns';
// Import ngx-barcode module
import { BarcodeGeneratorAllModule, DataMatrixGeneratorAllModule } from '@syncfusion/ej2-angular-barcode-generator';
import { ChartAllModule, AccumulationChartAllModule, RangeNavigatorAllModule } from '@syncfusion/ej2-angular-charts';
import { ChartsModule } from 'ng2-charts';
import { SwitchModule, RadioButtonModule } from '@syncfusion/ej2-angular-buttons';
import { GridAllModule } from '@syncfusion/ej2-angular-grids';
import { TreeGridAllModule, TreeGridModule } from '@syncfusion/ej2-angular-treegrid';
import { ButtonModule } from '@syncfusion/ej2-angular-buttons';

import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { AccountComponent } from './account/account.component';
import { BuildingModalComponent } from './building/building-modal/building-modal.component';
import { QRCodeGeneratorAllModule } from '@syncfusion/ej2-angular-barcode-generator';

import { MaskedTextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { HttpClient } from '@angular/common/http';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
// AoT requires an exported function for factories
export function HttpLoaderFactory(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}
import { setCulture, loadCldr, L10n } from '@syncfusion/ej2-schedule/node_modules/@syncfusion/ej2-base';
import { AutofocusDirective } from './focus.directive';
import { AutoSelectDirective } from './select.directive';
import { SearchDirective } from './search.directive';
import { SelectTextDirective } from './select.text.directive';
import { TooltipModule } from '@syncfusion/ej2-angular-popups';
import { TimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { DateTimePickerModule } from '@syncfusion/ej2-angular-calendars';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { SelectQrCodeDirective } from './select.qrcode.directive';
import { ButtonComponent } from './button/button.component';
import { FactoryComponent } from './factory/factory.component';
import { WorkerComponent } from './worker/worker.component';
import { WorkerListComponent } from './worker-list/worker-list.component';
import { CandleStickComponent } from './candle-stick/candle-stick.component';
setCulture('de-DE');
import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';
import { BuildingComponent } from './building/building.component';
const config: SocketIoConfig = { url: 'http://10.4.3.29:3486', options: {} };

const lang = localStorage.getItem('lang');
let defaultLang: any;
if (lang) {
  defaultLang = lang;
} else {
  defaultLang = 'en';
}
@NgModule({
  imports: [
    TreeGridAllModule,
    TreeGridModule,
    ButtonModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    ECRoutingModule,
    NgSelectModule,
    DropDownListModule,
    ChartsModule,
    ChartAllModule,
    AccumulationChartAllModule,
    RangeNavigatorAllModule,
    BarcodeGeneratorAllModule,
    QRCodeGeneratorAllModule,
    DataMatrixGeneratorAllModule,
    SwitchModule,
    MaskedTextBoxModule,
    DatePickerModule,
    GridAllModule,
    RadioButtonModule,
    TooltipModule,
    TimePickerModule ,
    Ng2SearchPipeModule,
    DateTimePickerModule,
    SocketIoModule.forRoot(config),
    TranslateModule.forChild({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpClient]
      },
      defaultLanguage: defaultLang
    }),
  ],
  declarations: [
    BuildingModalComponent,
    BuildingComponent,
    AccountComponent,
    AutofocusDirective,
    SelectTextDirective,
    AutoSelectDirective,
    SearchDirective,
    SelectQrCodeDirective,
    ButtonComponent,
    FactoryComponent,
    WorkerComponent,
    WorkerListComponent,
    CandleStickComponent
  ]
})
export class ECModule { }
