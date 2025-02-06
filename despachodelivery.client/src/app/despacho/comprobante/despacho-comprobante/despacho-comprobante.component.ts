import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FeltyRespuesta } from '../../../shared/interfaces/fletyResp.interface';
import { DespachoService } from '../../despacho.service';
import { saveAs } from 'file-saver';
@Component({
  selector: 'app-despacho-comprobante',
  templateUrl: './despacho-comprobante.component.html',
  styleUrl: './despacho-comprobante.component.css'
})
export class DespachoComprobanteComponent implements OnInit{
  
  dataPreEncript!: any
  jsonDataDesencript!: any
  jsonData!:any
  fletyResp!:FeltyRespuesta
  constructor(private activeRoute:ActivatedRoute,
    
  private despachoService:DespachoService) {
   
    
  }
  ngOnInit(): void {
    this.dataPreEncript = this.activeRoute.snapshot.queryParamMap.get('datos')
  
    this.jsonDataDesencript = atob(this.dataPreEncript)
    this.jsonData = JSON.parse(this.jsonDataDesencript)
    console.log(this.jsonData.value)
    console.log("hols" + this.jsonData.value)
    this.fletyResp = this.jsonData.value
    console.log(this.fletyResp.codCliente)
  }

  generarPdf(factura:String,almacen:String,despacho:Number){
    let dataComprobante = 
      {
        IdSucursal:almacen,
        NumFactura:factura,
        IdDespacho:despacho
    }
    
    this.despachoService.getComprobanteCliente(dataComprobante).subscribe(data=>{

     saveAs(data, `despacho-${factura}.pdf`)
    })
  }

}
