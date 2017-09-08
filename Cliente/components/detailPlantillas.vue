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
				<input :disabled="!isEditable" class="form-control" v-model="plantilla.Nombre" type="text" id="NombreInput" placeholder="Nombre"></input>
			</div>
			<div class="form-group">
				<label>Autor:</label>
				<input :disabled="!isEditable" class="form-control" type="text" v-model="plantilla.Autor" id="AutorInput" placeholder="Autor"></input>
			</div>
			<div class="form-group">
				<label>Estilo:</label>
				<select v-model="plantilla.Estilo" class="form-control" :disabled="!isEditable">
					<option value=1>Formal</option>
					<option value=2>Informal</option>
					<option value=3>Libro</option>
					<option value=4>Académico</option>
				</select>
			</div>
			<div class="form-group">
				<label>Tipo:</label>
				<select v-model="plantilla.Tipo" class="form-control" :disabled="!isEditable">
					<option value=1>Texto</option>
					<option value=2>Imagen</option>
					<option value=3>HTML</option>
					<option value=4>Hoja de cálculo</option>
				</select>
			</div>
			<label>	<input :disabled="!isEditable" class="checkbox" type="checkbox" v-model="plantilla.Accesibilidad" id="AccesibilidadInput" >Accesibilidad</label>
			<div class="form-group">
				<label>Precio (€):</label>
				<input :disabled="!isEditable" class="form-control" type="number" v-model="plantilla.Precio" id="precioInput" ></input>
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
				plantilla:{
				},
				previousPlantilla:{
				},
				isEditable:false,
				menuChoice : "Plantillas",
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
					/*if(this.plantilla.Nombre===this.previousPlantilla.Nombre){
						return  true; 
					}
					else if(this.plantilla.Autor===""){
						return true;
					}
					else if(this.plantilla.Tipo === "" ){
						return true;
					} 
					else if(this.plantilla.Precio === ""){
						return  true; 
					}
					else{
						return false;
					}*/
					return false;
				}
				else if(this.state == constantes.STATE_UPDATE){
					if(this.plantilla.Nombre != this.previousPlantilla.Nombre){
						return false;
					}
					else if(this.plantilla.Autor != this.previousPlantilla.Autor){
						return false;
					}
					else if(this.plantilla.Estilo != this.previousPlantilla.Estilo){
						return false;
					}
					else if(this.plantilla.Tipo != this.previousPlantilla.Tipo){
						return false;
					}
					else if(this.plantilla.Precio != this.previousPlantilla.Precio){
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
				this.previousPlantilla = $.extend({}, this.plantilla)
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
					if(this.plantilla.Nombre===""){
						errores+="El valor de Nombre está vacío. \n";
					}
					if(this.plantilla.Autor===""){
						errores+="El valor de Autor está vacío. \n";
					}
					if(this.plantilla.Estilo === "")
					{
						errores+="El valor de Estilo está vacío. \n";
					}
					if(this.plantilla.Tipo === "")
					{
						errores+="El valor de Tipo está vacío. \n";
					}
					if(this.plantilla.Precio === 0){
						errores+="El valor de Precio no es correcto. \n";	
					}
					if(errores != ""){
						alert("Hay campos no rellenados. No se puede crear el objeto:\n" + errores);
					}
					else{
						$.ajax({url:constantes.BASE_URL + this.menuChoice,
							method:"POST",
							data: this.plantilla})	
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
						data: this.plantilla})
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
				this.previousPlantilla = $.extend({}, this.plantilla);
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
					this.plantilla = {};
					this.currentId = "";
					this.plantilla.Nombre = "";
					this.plantilla.Autor = "";
					this.plantilla.Estilo = "";
					this.plantilla.Tipo = 0;
					this.plantilla.Precio = 0;
					this.plantilla.Accesibilidad = false;


					this.previousPlantilla={};
					this.previousPlantilla.Nombre = "";
					this.previousPlantilla.Autor = "";
					this.previousPlantilla.Estilo = "";
					this.previousPlantilla.Tipo = "";
					this.previousPlantilla.Tipo = 0;
					this.previousPlantilla.Precio = 0;
					this.previousPlantilla.VersionFinal = false;
					this.previousPlantilla.SoloLectura = false;
					this.previousPlantilla.Accesibilidad = false;
				}
			},
			submitGetRequest(datos){
				this.currentId = datos.Id;
				this.plantilla = datos; 	
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