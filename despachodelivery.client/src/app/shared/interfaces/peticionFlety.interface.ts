export interface  PeticionDespacho{
    InvoiceNumber?:     string;
    Sucursal?:        string;
    InvoiceDate:       Date;
    Cedula?:            string;
    StoreCode?:         string;
    CorreoCliente?:     string;
    NameClient?:        string;
    SurnameClient?:     string;
    PhoneNumberClient?: string;
    AddressClient?:     string;
    Articles?:          Article[];
    Observation?:       string;
}

export interface Article {
    codarticulo: string;
    nomArticulo: string;
}
