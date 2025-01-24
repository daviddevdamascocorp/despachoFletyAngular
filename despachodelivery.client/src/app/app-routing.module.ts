import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ProcesoDespachoComponent } from './despacho/proceso-despacho/proceso-despacho.component';
import { DespachoComprobanteComponent } from './despacho/comprobante/despacho-comprobante/despacho-comprobante.component';

const routes: Routes = [
  {path:'despacho',component:ProcesoDespachoComponent},
  {path:'comprobante',component:DespachoComprobanteComponent},
  {
    path: '',
    redirectTo: 'despacho',
    pathMatch: 'full'
  }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
