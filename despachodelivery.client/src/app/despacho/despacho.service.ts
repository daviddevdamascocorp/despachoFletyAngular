import { Injectable } from '@angular/core';
import { environment } from '../../enviroments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Tienda } from '../shared/interfaces/tiendas.interface';
import { FacturaHeadKlk } from '../shared/interfaces/facturaklkheader.interface';
import { PeticionDespacho } from '../shared/interfaces/peticionFlety.interface';

@Injectable({
  providedIn: 'root'
})

export class DespachoService {

   url = environment.baseUrl
  constructor(private httpClient:HttpClient) { }

  getTiendasDamasco():Observable<Tienda[]>{
    return   this.httpClient.get<Tienda[]>(`${this.url}/api/DespachoCliente/tiendas`)
  }

  getDetalleCliente(factura:String,almacen:String | undefined){
    return   this.httpClient.get<FacturaHeadKlk[]>(`${this.url}/api/DespachoCliente/factura/${factura}/almacen/${almacen}`)
  }

  postClienteInfo(bodyPeticion:any){
    return this.httpClient.post(`${this.url}/api/DespachoCliente/subir`,bodyPeticion)
  }
  getComprobanteCliente(comprobanteData:any){
    return   this.httpClient.post(`${this.url}/api/DespachoCliente/getpdf`, comprobanteData, { responseType: 'blob' })
  }
}
