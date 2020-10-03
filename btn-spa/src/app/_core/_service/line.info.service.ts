import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
const httpOptions = {
  headers: new HttpHeaders({
    'Content-Type': 'application/json',
    Authorization: 'Bearer ' + localStorage.getItem('token')
  })
};
@Injectable({
  providedIn: 'root'
})
export class LineInfoService {
  baseUrl = environment.apiUrlEC;
  authUrl = environment.apiUrl;
  messageSource = new BehaviorSubject<number>(0);
  currentMessage = this.messageSource.asObservable();
  // method này để change source message
  changeMessage(message) {
    this.messageSource.next(message);
  }
  constructor(private http: HttpClient) { }
  create(model) {
    return this.http.post(this.baseUrl + 'LineInfo/Create', model);
  }
  update(model) {
    return this.http.put(this.baseUrl + 'LineInfo/Update', model);
  }
  delete(id: number) {
    return this.http.delete(this.baseUrl + 'LineInfo/Delete/' + id);
  }
  getLineInfoByLine(lineName: string) {
    return this.http.get(this.baseUrl + 'LineInfo/GetLineInfoByLine/' + lineName);
  }
}
