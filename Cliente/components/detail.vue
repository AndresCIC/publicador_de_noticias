<template>
	<div id="Detail" class="container-fluid">
		<div id ="FormularioDocumentos" class="mx-auto" style="width: 45%">
			<center>
				<div class="form-group">
					<button :disabled="this.isEditable" id="enableEditButton" class="btn btn-primary" v-on:click="buttonEnableEdit">Modificar</button>
					<button  id="cancelarButton" class="btn btn-primary" v-on:click="buttonCancelar">Cancelar</button>

				</div>
			</center>
			<div class="form-group">
				<label for="Nombre">Título:</label>
				<input :disabled="!isEditable" class="form-control" v-model="documento.Titulo" type="text" id="TituloInput" placeholder="Titulo"></input>
			</div>
			<div class="form-group">
				<label>Autor:</label>
				<input :disabled="!isEditable" class="form-control" type="text" v-model="documento.Autor" id="AutorInput" placeholder="Autor"></input>
			</div>
			<div class="form-group row">
				<div class="col">
					<label>Fecha de creación:</label>
					
					<input :disabled="!isEditable" class="form-control" type="text" v-model="documento.FechaCreacion" id="creacionInput" ></input>
					
				</div>
				<div class="col">
					<label>Última modificación:</label>
					<input :disabled="!isEditable" class="form-control" type="text" v-model="documento.FechaUltimoModificado" id="ultimamodificacionInput"></input>
				</div>
			</div>
			<div class="form-group">
				<label>Tamaño:</label>
				<input :disabled="!isEditable" class="form-control" type="number" v-model="documento.Tamanio" id="tamanioInput" ></input>
			</div>
			<div class="form-group">
				<label>Tipo:</label>
				<select v-model="documento.Tipo" class="form-control" :disabled="!isEditable">
					<option value=1>Texto</option>
					<option value=2>Imagen</option>
					<option value=3>HTML</option>
					<option value=4>Hoja de cálculo</option>
				</select>
			</div>
			<div class="checkbox">
				<label>	<input :disabled="!isEditable" class="checkbox" type="checkbox" v-model="documento.SoloLectura" id="modificableInput" >Solo lectura</label>
				<label>	<input :disabled="!isEditable" class="checkbox" type="checkbox" v-model="documento.VersionFinal" id="desarrolloInput" >En desarrollo</label>
			</div>
			<center>
				<div class="form-group">
					<button  id="acceptButton" :disabled="this.computeAcceptButton" class="btn btn-primary" v-on:click="buttonAccept">Aceptar</button>
					<button  id="borrarButton" :disabled="this.computeDeleteButton" class="btn btn-primary" v-on:click="buttonBorrar">Borrar</button>
				</div>
			</center>

		</div>
	</div>
</template>

<script>
	import { EventBus } from './eventBus.js';
	import constantes from './constants.js';

	export default{
		created() {
			EventBus.$on('enableDetail', read => {
				this.read = read;
			});
		},	
		components:{
		},
		data (){
			return{
				documento:{
				},
				previousDocument:{
				},
				isEditable:false,
				menuChoice : "Documentos",
				estaVacio : false

			}
		},
		props:[
		'state',
		'currentId'
		],
		computed:{
			computeAcceptButton: function()	{
				if(!this.isEditable){
					return  true; 
				}
				else if(this.state == constantes.STATE_NEW){
					/*if(this.documento.Titulo===this.previousDocument.Titulo){
						return  true; 
					}
					else if(this.documento.Autor===""){
						return true;
					}
					else if(this.documento.Tipo === "" ){
						return true;
					} 
					else if(this.documento.Tamanio === ""){
						return  true; 
					}
					else{
						return false;
					}*/
					return false;
				}
				else if(this.state == constantes.STATE_UPDATE){
					if(this.documento.Titulo != this.previousDocument.Titulo){
						return false;
					}
					else if(this.documento.Autor != this.previousDocument.Autor){
						return false;
					}
					else if(this.documento.FechaCreacion != this.previousDocument.FechaCreacion){
						return false;
					}
					else if(this.documento.FechaUltimoModificado != this.previousDocument.FechaUltimoModificado){
						return false;
					}
					else if(this.documento.Tipo != this.previousDocument.Tipo){
						return false;
					}
					else if(this.documento.Tamanio != this.previousDocument.Tamanio){
						return false;
					}
					else if(this.documento.SoloLectura != this.previousDocument.SoloLectura){
						return false;
					}
					else if(this.documento.VersionFinal != this.previousDocument.VersionFinal){
						return false;
					}
					else{return true;}
				}
			},
			computeDeleteButton: function(){
				if(!this.isEditable){
					return true;
				}
				else if(this.state == constantes.STATE_UPDATE){
					return false;
				}
				else {return true};
			},
		},
		methods:{
			buttonEnableEdit: function(){
				this.isEditable = !this.isEditable;
				this.previousDocument = $.extend({}, this.documento)
			},
			buttonBorrar: function(){
				if(confirm("¿Está seguro de que quiere borrar?")){
					//TODO: Peticion AJAX
					$.ajax({
						url:constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
						method: "DELETE"
					})
					.done(this.borradoHandler)
					.fail(function(){alert("Ha habido un error al borrar.");})
				}
			},
			buttonCancelar: function(){
				this.$emit('cancelDetail', true);

			},
			borradoHandler: function(){
				alert("Elemento borrado correctamente.");
				this.$emit('makeGet', true);
				this.makeEmptyData();
			},
			buttonAccept: function(){

				if(this.state == constantes.STATE_NEW){
					let errores = "";
					if(this.documento.Titulo===""){
						errores+="El valor de Título está vacío. \n";
					}
					if(this.documento.Autor===""){
						errores+="El valor de Autor está vacío. \n";
					}
					if(this.documento.FechaCreacion === "")
					{
						errores+="El valor de Fecha de Creación está vacío. \n";
					}
					if(this.documento.FechaUltimoModificado === "")
					{
						errores+="El valor de Última Modificación está vacío. \n";
					}
					if(this.documento.Tipo === 0){
						errores+="El valor de Tipo no es correcto. \n";	
					} 
					if(this.documento.Tamanio === 0){
						errores+="El valor de Tamaño es 0. \n";
					}
					if(errores != ""){
						alert("Hay campos no rellenados. No se puede crear el objeto:\n" + errores);
					}
					else{
						$.ajax({url:constantes.BASE_URL + this.menuChoice,
							method:"POST",
							data: this.documento})	
						.done(this.afterPostHandler)
						.fail(function(){
							alert("Fallo en la creacion del elemento");
						//TODO: Gestionar los fallos
					})
					}

				}
				else if(this.state == constantes.STATE_UPDATE){
					// TODO: Se hace un PUT con el objeto

					$.ajax({url:constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
						method:"PUT",
						data: this.documento})
					.done(this.putSubmitData)
				}
			},
			afterPostHandler(){
				alert("Elemento creado");
				this.$emit('forceUpdate', true);

				// TODO: Se fuerza un get en el maestro y se cierra el detail.
				// Podemos llamar al metodo buttonCancelar.
			},
			putSubmitData(){
				alert("Elemento modificado");
				this.previousDocument = $.extend({}, this.documento);
				this.$emit('forceUpdate', true);

			}, 	
			makeGetRequest(){
				$.ajax({
					url: constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
					method: "GET"
				})
				.done(this.submitGetRequest)
				.fail(function(){
					alert("Ha fallado la carga del objeto");
				})
			},
			makeEmptyData(){
				if(!this.estaVacio){
					this.documento = {};
					this.currentId = "";
					this.documento.Titulo = "";
					this.documento.Autor = "";
					this.documento.FechaCreacion = "";
					this.documento.FechaUltimoModificado = "";
					this.documento.Tipo = 0;
					this.documento.Tamanio = 0;
					this.documento.VersionFinal = false;
					this.documento.SoloLectura = false;

					this.previousDocument={};
					this.previousDocument.Titulo = "";
					this.previousDocument.Autor = "";
					this.previousDocument.FechaCreacion = "";
					this.previousDocument.FechaUltimoModificado = "";
					this.previousDocument.Tipo = 0;
					this.previousDocument.Tamanio = 0;
					this.previousDocument.VersionFinal = false;
					this.previousDocument.SoloLectura = false;
				}
			},
			submitGetRequest(datos){
				this.currentId = datos.Id;
				this.documento = datos; 	
			},
			parseTipo: function(array){
				var _this = this;
				array.forEach(function(element, index) {
					if(element.Tipo == 4){
						_this.lista[index].Tipo = "Texto";
					}
					else if(element.Tipo == 1){
						_this.lista[index].Tipo = "Imagen";
					}
					else if(element.Tipo == 2){
						_this.lista[index].Tipo = "HTML";
					}
					else if(element.Tipo == 3){
						_this.lista[index].Tipo = "Hoja de cálculo";
					}
				});
			},
			makeNewDetail: function(){
				this.makeEmptyData();
				this.currentId = "";
				this.state = constantes.STATE_NEW;
				this.isEditable = true;
			},
			
		}, 
		mounted(){
			if(this.state == constantes.STATE_UPDATE){
				this.makeGetRequest();
			}
			else if(this.state == constantes.STATE_NEW){
				this.makeEmptyData(); 	
				this.estaVacio = true;
				this.isEditable = true;
			}
		},
		updated(){
		/*	if(this.state == constantes.STATE_NEW){
				this.makeNewDetail();
			}*/
		}
	}
</script>

<style></style>