import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { PaginatedResult } from '../_model/pagination';
@Injectable({
  providedIn: 'root'
})
export class ButtonService {
  baseUrl = environment.apiUrlEC;
  ModalNameSource = new BehaviorSubject<number>(0);
  currentModalName = this.ModalNameSource.asObservable();
  authUrl = environment.apiUrl;
  constructor(
    private http: HttpClient
  ) { }

  getAllButton() {
    return this.http.get(this.baseUrl + 'Button/GetAll', {});
  }
  getAllButtonByWorkerID(workerID) {
    return this.http.get(this.baseUrl + 'Button/GetAllButtonByWorkerID/' + workerID, {});
  }
  create(model) {
    return this.http.post(this.baseUrl + 'Button/Create', model);
  }
  update(model) {
    return this.http.put(this.baseUrl + 'Button/Update', model);
  }
  delete(id: number) {
    return this.http.delete(this.baseUrl + 'Button/Delete/' + id);
  }
  getAllUsers(page?, pageSize?): Observable<PaginatedResult<object[]>> {
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

  createWorkerButton(model) {
    return this.http.post(this.baseUrl + 'Button/CreateWorkerButton', model);
  }
  updateWorkerButton(model) {
    return this.http.put(this.baseUrl + 'Button/UpdateWorkerButton', model);
  }
  unlinkWorkerWithButton(workerID) {
    return this.http.get(this.baseUrl + 'Button/UnlinkWorkerWithButton/' + workerID);
  }
  checkExistWorkerLinkButton(workerID) {
    return this.http.get(`${this.baseUrl}Button/CheckExistWorkerLinkButton/${workerID}`);
  }
  unlinkButtonLinkWorker(btn) {
    return this.http.get(this.baseUrl + 'Button/UnlinkButtonLinkWorker/' + btn);
  }
  checkExistButtonLinkWorker(btn) {
    return this.http.get(`${this.baseUrl}Button/CheckExistButtonLinkWorker/${btn}`);
  }
  deleteWorkerButton(id: number) {
    return this.http.delete(this.baseUrl + 'Button/DeleteWorkerButton/' + id);
  }
}
