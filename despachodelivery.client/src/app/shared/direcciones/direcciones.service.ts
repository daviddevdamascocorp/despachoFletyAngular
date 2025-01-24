import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../enviroments/environment.development';
import { Observable } from 'rxjs';
import { Ciudad, Estado, Municipio,  Zona } from '../interfaces/estados.interfafe';

@Injectable({
  providedIn: 'root'
})
export class DireccionesService {
 url = environment.baseUrl
  constructor(private httpClient:HttpClient) { }

 getEstado():Observable<Estado[]>{
      return this.httpClient.get<Estado[]>(`${this.url}/api/direcciones/estado/1`)
 }

 getMunicipio(estado:string):Observable<Municipio[]>{
      return   this.httpClient.get<Municipio[]>(`${this.url}/api/direcciones/municipio/${estado}`)
 }

 getCiudad(estado:string | undefined):Observable<Ciudad[]>{
      return   this.httpClient.get<Ciudad[]>(`${this.url}/api/direcciones/ciudad/${estado}`)
 }

 getZona(estado:string | undefined, municipio:string | undefined):Observable<Zona[]>{
      return   this.httpClient.get<Zona[]>(`${this.url}/api/direcciones/zona/${estado}/num/${municipio}`)
 }
 
}
