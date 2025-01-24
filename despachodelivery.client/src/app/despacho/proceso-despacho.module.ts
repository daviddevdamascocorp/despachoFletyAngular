import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProcesoDespachoRoutingModule } from './proceso-despacho-routing.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MaterialModule } from '../shared/material/material.module';
import { MatSelectModule } from '@angular/material/select';
import { ProcesoDespachoComponent } from './proceso-despacho/proceso-despacho.component';
import { DespachoComprobanteComponent } from './comprobante/despacho-comprobante/despacho-comprobante.component';
import { LoaderModModule } from '../shared/loader-mod/loader-mod.module';



@NgModule({
  declarations: [ProcesoDespachoComponent, DespachoComprobanteComponent],
  imports: [
    CommonModule,
    ProcesoDespachoRoutingModule,
    ReactiveFormsModule,
    MaterialModule,
    MatSelectModule,
    LoaderModModule
  ]
})
export class ProcesoDespachoModule { }
