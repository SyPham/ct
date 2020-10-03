import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { PaginatedResult } from '../_model/pagination';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
@Injectable({
  providedIn: 'root'
})
export class WorkerService {
  baseUrl = environment.apiUrlEC;
  authUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient) { }
  getAllUsers(page?, pageSize? ): Observable<PaginatedResult<object[]>> {
    const paginatedResult: PaginatedResult<object[]> = new PaginatedResult<
    object[]
    >();
    return this.http
      .get<object[]>(`${this.authUrl}Users/GetAllUsers/${page}/${pageSize}`, {
        observe: 'response'
      })
      .pipe(
        map(response => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
              );
            }
          return paginatedResult;
        })
      );
  }
  getAllWorkers() {
    return this.http.get(`${this.baseUrl}Worker/GetAll`);
  }
  create(model) {
    return this.http.post(this.baseUrl + 'Worker/Create', model);
  }
  update(model) {
    return this.http.put(this.baseUrl + 'Worker/Update', model);
  }
  updateTalkTime(model) {
    return this.http.put(this.baseUrl + 'Worker/UpdateTaktTime', model);
  }
  updateOperation(model) {
    return this.http.put(this.baseUrl + 'Worker/UpdateOperation', model);
  }
  updateStandard(model) {
    return this.http.put(this.baseUrl + 'Worker/UpdateStandard', model);
  }
  delete(id: number) {
    return this.http.delete(this.baseUrl + 'Worker/Delete/' + id);
  }
  getWorkersByLine(id: string) {
    return this.http.get(this.baseUrl + 'Worker/GetWorkersByLine/' + id);
  }
  chart(workerID: number, btnID: number) {
    return this.http.get(`${this.baseUrl}Worker/Chart/${workerID}/${btnID}` );
  }
  excelExport(workerID: number, btnID: number) {
    return this.http.get(`${this.baseUrl}Worker/ExcelExport/${workerID}/${btnID}`, { responseType: 'blob' });
  }
  deleteUser(id) { return this.http.delete(`${this.authUrl}Users/Delete/${id}`); }
  mapBuildingUser(userid, buildingid) { return this.http.get(`${this.baseUrl}BuildingUser/MapBuildingUser/${userid}/${buildingid}`); }
  getBuildings() { return this.http.get(`${this.baseUrl}Building/GetBuildings`); }
  getLines() { return this.http.get(`${this.baseUrl}Building/GetLines`); }
  updateUser(update) { return this.http.post(`${this.authUrl}Users/Update`, update); }
  createUser(create) { return this.http.post(`${this.authUrl}Users/Create`, create); }
  getBuildingUsers() {
      return this.http.get(`${this.baseUrl}BuildingUser/GetAllBuildingUsers`);
  }
}
