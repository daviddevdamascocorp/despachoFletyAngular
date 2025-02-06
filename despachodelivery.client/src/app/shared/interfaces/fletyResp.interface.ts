export interface FeltyRespuesta {
    idDespacho:Number;
    numFactura:         string;
    codCliente:         string;
    nomCliente:         string;
    articulos:          Articulo[];
    status:             string;
    sucursal:           string;
    numeroTelefono:     string;
    fechaFactura:       Date;
    fechaActualizacion: Date;
    direccion:          string;
    observaciones:      string;
    idSucursal : string;
}

export interface Articulo {
    codArticulo:      string;
    descripcion:      string;
    cantidadArticulo: number;
}
