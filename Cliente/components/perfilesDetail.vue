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
				<label for="Nombre">Nombre:</label>
				<input :disabled="!isEditable" class="form-control" v-model="perfil.Nombre" type="text" id="NombreInput" placeholder="Nombre"></input>
			</div>
			<div class="form-group">
				<label>Permisos:</label>
				<select :disabled="!isEditable" class="form-control" v-model="perfil.Permisos" id="permisosInput" >
				  <option value=1>Lectura</option>
				  <option value=2>Lectura + Escritura</option>
				  <option value=3>Total </option>
				</select>
			</div>
		
			<div class="form-group">
				<label>Extensión de correo electrónico</label>
				<input :disabled="!isEditable" class="form-control" type="email" v-model="perfil.ExtCorreoE" id="ExtCorreoEInput" ></input>
			</div>
			
			<div class="form-group">
				<label>Descripción</label>
				<textarea rows="4" cols="50" :disabled="!isEditable" class="form-control" v-model="perfil.Descripcion" id="descripcionInput"></textarea>
			</div>
			<div class="checkbox">
				<label>	<input :disabled="!isEditable" class="checkbox" type="checkbox" v-model="perfil.Activo" id="modificableInput" >Activo</label>
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
				perfil:{
				},
				previousPerfil:{
				},
				isEditable:false,
				menuChoice : "PerfilesUsuarios",
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
					if(this.perfil.Nombre != this.previousDocument.Titulo){
						return false;
					}
					else if(this.perfil.Permisos != this.previousDocument.Autor){
						return false;
					}
					else if(this.perfil.ExtCorreoE != this.previousDocument.FechaCreacion){
						return false;
					}
					else if(this.perfil.Descripción != this.previousDocument.FechaUltimoModificado){
						return false;
					}
					else if(this.perfil.Activo != this.previousDocument.Tipo){
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
				this.previousDocument = $.extend({}, this.perfil)
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
					if(this.perfil.Nombre===""){
						errores+="El valor del nombre está vacío. \n";
					}
					if(this.perfil.Permisos===""){
						errores+="El valor de Autor está vacío. \n";
					}
					if(this.perfil.ExtCorreoE === "")
					{
						errores+="El valor de Fecha de Creación está vacío. \n";
					}
					if(this.perfil.Activo === "")
					{
						errores+="El valor de Última Modificación está vacío. \n";
					}
					if(this.perfil.Descripcion === 0){
						errores+="El valor de Tipo no es correcto. \n";	
					}
					if(errores != ""){
						alert('No puede hacer campos vacios!');						
					}
					else{
						$.ajax({url:constantes.BASE_URL + this.menuChoice,
							method:"POST",
							data: this.perfil})	
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
						data: this.perfil})
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
				this.previousDocument = $.extend({}, this.perfil);
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
					this.perfil = {};
					this.currentId = "";
					this.perfil.Nombre = "";
					this.perfil.Permisos = "";
					this.perfil.Descripción = "";
					this.perfil.Activo = false;

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
				this.perfil = datos; 	
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
				//this.makeEmptyData(); 	
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