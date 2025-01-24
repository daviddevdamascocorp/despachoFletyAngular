import { Component, OnInit } from '@angular/core';
import { Tienda } from '../../shared/interfaces/tiendas.interface';
import { DespachoService } from '../despacho.service';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { Article, PeticionDespacho } from '../../shared/interfaces/peticionFlety.interface';
import { MyErrorStateMatcher } from '../../shared/matcher';
import { FacturaHeadKlk } from '../../shared/interfaces/facturaklkheader.interface';
import { Router } from '@angular/router';
import { Ciudad, Estado, Municipio, Zona } from '../../shared/interfaces/estados.interfafe';
import { DireccionesService } from '../../shared/direcciones/direcciones.service';
import { MatSelectChange } from '@angular/material/select';
import { concatWith } from 'rxjs';

@Component({
  selector: 'app-proceso-despacho',
  templateUrl: './proceso-despacho.component.html',
  styleUrl: './proceso-despacho.component.css'
})
export class ProcesoDespachoComponent implements OnInit{

  /**
   *
   */
  DamascoTiendas!:Tienda[]
 public header!:FacturaHeadKlk
 public articulos!:FacturaHeadKlk[]
 public peticionDespacho!:PeticionDespacho
 estados!:Estado[]
 municipios!:Municipio[]
 zonas!:Zona[]
 ciudades!:Ciudad[]
  constructor(private despachoService:DespachoService, private router:Router, private direccionService:DireccionesService) {

    
  }
  matcher = new MyErrorStateMatcher();
  ngOnInit(): void {
      this.despachoService.getTiendasDamasco().subscribe(result=>{
        this.DamascoTiendas = result
        console.log(result);
        
  })

      this.direccionService.getEstado().subscribe(result=>{
        this.estados = result
        console.log(this.estados)
      })
  }
  cedulas = ['V','J','E']
  telefonos = ['0412','0414','0424','0416','0426']

  clientesForm = new FormGroup({
    InvoiceNumber: new FormControl('', [Validators.required]),
      Sucursal: new FormControl('', [Validators.required]),
      InvoiceDate: new FormControl('', [Validators.required]),
      TipoCedula:  new FormControl('', [Validators.required]),
      Cedula: new FormControl('', [Validators.required, Validators.pattern(/^\d+$/)]), // Validate for numbers only
      StoreCode: new FormControl(''),
      CorreoCliente: new FormControl('', [Validators.required, Validators.email]),
      NameClient: new FormControl('', [Validators.required]),
      SurnameClient: new FormControl('', [Validators.required]),
      TypePhone: new FormControl('', [Validators.required]),
      PhoneNumberClient: new FormControl('', [Validators.required]),
      estadoSelect: new FormControl('', [Validators.required]),
      municipioSelect: new FormControl('', [Validators.required]),
      ciudadSelect: new FormControl('', [Validators.required]),
      zonaSelect: new FormControl('', [Validators.required]),
      estado: new FormControl('', [Validators.required]),
      municipio: new FormControl('', [Validators.required]),
      ciudad: new FormControl('', [Validators.required]),
      zona: new FormControl('', [Validators.required]),
      urbanizacion: new FormControl('', [Validators.required]),
      calle: new FormControl('', [Validators.required]),
      casa:new FormControl('', [Validators.required]),
      piso: new FormControl(''),
      apto: new FormControl(''),
      AddressClient: new FormControl(''),
      Articles: new FormArray([],[Validators.required]), 
      Observation: new FormControl(''),
  })

  get articles(): FormArray {
    return this.clientesForm.get('Articles') as FormArray;
  }
  agregarArticulo(articulo: FacturaHeadKlk): void {
    const articleFormGroup = new FormGroup({
      codarticulo: new FormControl(articulo.codArticulo),
      nomArticulo: new FormControl(articulo.descripcion),
    });
    this.articles.push(articleFormGroup);
  }

  getMunicipio(ev:MatSelectChange){
     this.clientesForm.patchValue({municipio:ev.source.triggerValue})
    this.direccionService.getMunicipio(ev.value).subscribe(res => this.municipios = res)
  }
  getCiudad(ev:MatSelectChange){
    let estado = this.clientesForm.get('estadoSelect')?.value

   this.clientesForm.patchValue({ciudad:ev.source.triggerValue})
   console.log(estado);
   
    this.direccionService.getCiudad(estado?.toString()).subscribe(res=>{ this.ciudades = res
      console.log(this.ciudades)
    })
  }
  getZona(ev:MatSelectChange){
    let municipio = this.clientesForm.get('municipioSelect')?.value
    let estado = this.clientesForm.get('estadoSelect')?.value
   
    this.direccionService.getZona(estado?.toString(),municipio?.toString()).subscribe(res=> this.zonas = res)
  }

  setZona(ev:MatSelectChange){
    this.clientesForm.patchValue({zona:ev.source.triggerValue})
  }
  setEstado(ev:MatSelectChange){
    this.clientesForm.patchValue({estado:ev.source.triggerValue})
  }

  procesarFactura(ev:Event){
    let numFactura = ev.target as HTMLInputElement
    let valorNumFactura = numFactura.value
    let Sucursal = this.clientesForm.get('Sucursal')?.value
    
    if (Sucursal !== null && Sucursal !== undefined) {
      this.despachoService.getDetalleCliente(valorNumFactura,Sucursal).subscribe(result=>{
       this.header = result[0]
       this.articulos = result
       
       let fecha =  this.header.fechaFactura.toString()
       let fechaForm = fecha.split('T')
      let nombreSeparado =  this.header.nomCliente.trim().split(' ');
      
      let articles = (this.articulos).map(articulo=>({
       
        codarticulo:articulo.codArticulo,
        nomArticulo:articulo.descripcion
     }))
     console.log(articles)
       this.clientesForm.patchValue({
        InvoiceDate:fechaForm.at(0),
        NameClient:nombreSeparado[0],
        SurnameClient:nombreSeparado.slice(1).join(' '),
        TipoCedula:this.header.codCliente.charAt(0),
        Cedula:this.header.codCliente.substring(2),
        
       })

       result.forEach(articulo=>{
        this.agregarArticulo(articulo)
       })
    
        
      })
    }
    
  }



  sendDespacho(){
    let direccionCompleta = "Estado: " + this.clientesForm.get('estado')?.value + ' ' +" Municipio: "+ this.clientesForm.get('municipio')?.value
    +" "+ this.clientesForm.get('ciudad')?.value +" " + this.clientesForm.get('zona')?.value +" Urbanización: "+ this.clientesForm.get('urbanizacion')?.value
    + " " +this.clientesForm.get('calle')?.value + " " +"Casa/Edificio: "+ this.clientesForm.get('casa')?.value + " " + `Piso:${this.clientesForm.get('piso')?.value}  Apartamento:${this.clientesForm.get('apto')?.value}`  
     
    console.log(direccionCompleta)
    let sucursal  =   this.clientesForm.get('Sucursal')?.value
    let factura = this.clientesForm.get('InvoiceNumber')?.value
    let cedula =  this.clientesForm.get('Cedula')?.value 
    let numero = this.clientesForm.get('TypePhone')?.value?.toString() +''+ this.clientesForm.get('PhoneNumberClient')?.value?.toString()
  let articleArrayForm =  this.clientesForm.get('Articles') as FormArray
  let listaArticulo:Article[] = articleArrayForm.value.map((article: any) => ({
    codarticulo: article.codarticulo,
    nomArticulo: article.nomArticulo,
  }));
   
    
   let data ={AddressClient :  direccionCompleta?.toString(),

    InvoiceNumber : factura?.toString(),
    Sucursal : sucursal?.toString(),
    Cedula : cedula?.toString(),
    CorreoCliente : this.clientesForm.get('CorreoCliente')?.value?.toString(),
    StoreCode : sucursal?.toString(),
    NameClient : this.clientesForm.get('NameClient')?.value?.toString(),
    SurnameClient : this.clientesForm.get('SurnameClient')?.value?.toString(),
    PhoneNumberClient : numero,
   Observation: this.clientesForm.get('Observation')?.value?.toString(),
    Articles : listaArticulo} 
    console.log(data)
    this.despachoService.postClienteInfo(data).subscribe(result=>{
      let response = JSON.stringify(result)
      this.router.navigate(['/comprobante'],{queryParams:{datos:btoa(response)}})
   
    })
  }

 
}
