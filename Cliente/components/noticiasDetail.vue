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
				<label>Título:</label>
				<input required :disabled="!isEditable" class="form-control" type="text" v-model="noticia.Titulo" id="AutorInput" placeholder="Introduzca título"></input>
			</div>
			<div class="form-group">
				<label>Tipo:</label>
				<select required v-model="noticia.Tipo" class="form-control" :disabled="!isEditable">
				Cronologica, InteresHumano, Futuro, Espacial 
					<option value=1>Cronológica</option>
					<option value=2>Interés humano</option>
					<option value=3>Futuro</option>
					<option value=4>Espacial</option>
				</select>
			</div>
			
			<div class="form-group">
				<label>Fecha:</label>
				<input required :disabled="!isEditable" class="form-control" type="datetime-local" v-model="noticia.FechaPublicacion" id="FechaPublicacionInput"></input>
			</div>
			<div class="form-group">
				<label>Contenido</label>
				<textarea required rows="4" cols="50" :disabled="!isEditable" class="form-control" v-model="noticia.Contenido" id="contenidoInput" placeholder="Introduzca el contenido"></textarea>
			</div>

			<label>	<input :disabled="!isEditable" class="checkbox" type="checkbox" v-model="noticia.Publicado" id="PublicarInput">Publicar</label>
			<center>
				<div class="form-group">
					<button  id="acceptButton" :disabled="this.computeAcceptButton" class="btn btn-primary" v-on:click="buttonAccept">Aceptar</button>

					 <button id="borrarButton" :disabled="this.computeDeleteButton" class="btn btn-danger" v-on:click="showModal = true, buttonBorrar()">Borrar</button>
					  <!-- use the modal component, pass in the prop -->
					  <modal v-if="showModal" @close="showModal = false">
					    <!--
					      you can use custom content here to overwrite
					      default content
					    -->
					    <h4 slot="header">Se requiere confirmación</h4>
				    	<p slot="body"> ¿Está seguro que quiere borrar esta noticia? </p>
					    
					  </modal>
				</div>
			</center>

		</div>
	</div>
</template>

<script>
	var SizeEnum = {
	  SMALL: 1,
	  MEDIUM: 2,
	  LARGE: 3,
	};
	import constantes from './constants.js';
	import modal  from './modal.vue';
	import { EventBus } from './eventBus.js';
	export default{
		created() {
			EventBus.$on('enableDetail', read => {
				this.read = read;
			});
		},	
		components:{
			modal
		},
		data (){
			return{
				noticia:{
				},
				previousNoticia:{
				},
				isEditable:false,
				menuChoice : "Noticias",
				estaVacio : false,
				showModal : false
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
					return false;
				}
				else if(this.state == constantes.STATE_UPDATE){
					if(this.noticia.Nombre != this.previousNoticia.Nombre){
						return false;
					}
					else if(this.noticia.Autor != this.previousNoticia.Autor){
						return false;
					}
					else if(this.noticia.Estilo != this.previousNoticia.Estilo){
						return false;
					}
					else if(this.noticia.Tipo != this.previousNoticia.Tipo){
						return false;
					}
					else if(this.noticia.Precio != this.previousNoticia.Precio){
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
				this.previousNoticia = $.extend({}, this.noticia)
			},
			buttonBorrar: function(){
				EventBus.$once('Confirmation', confirm => {
					if(confirm === 'OK'){
						$.ajax({
								url:constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
								method: "DELETE"
							})
							.done(this.borradoHandler(this))
							.fail(EventBus.$emit('error', 'Se ha producido un error al borrar'))
						}
								
				});					
			},
			buttonCancelar: function(){
				this.$emit('cancelDetail', true);

			},
			borradoHandler: function(){
				EventBus.$emit('Message', 'success', 'Se ha borrado correctamente');
				this.$emit('makeGet', true);
				this.makeEmptyData();
			},
			buttonAccept: function(){
				if (this.state == constantes.STATE_UPDATE){
				// TODO: Se hace un PUT con el objeto

				$.ajax({url:constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
					method:"PUT",
					data: this.noticia})
				.done(this.putSubmitData);

				}
				else
				{
				$.ajax({url:constantes.BASE_URL + this.menuChoice,
					method:"POST",
					data: this.noticia})	
				.done(this.afterPostHandler)
				.fail(function(){
					alert("Fallo en la creacion del elemento");
				})
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
				this.previousNoticia = $.extend({}, this.noticia);
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
					this.noticia = {};
					//this.currentId = "";
					this.noticia.Nombre = "";
					this.noticia.Autor = "";
					this.noticia.Estilo = "";
					this.noticia.Tipo = 0;
					this.noticia.Precio = 0;
					this.noticia.Accesibilidad = false;


					this.previousNoticia={};
					this.previousNoticia.Nombre = "";
					this.previousNoticia.Autor = "";
					this.previousNoticia.Estilo = "";
					this.previousNoticia.Tipo = "";
					this.previousNoticia.Tipo = 0;
					this.previousNoticia.Precio = 0;
					this.previousNoticia.VersionFinal = false;
					this.previousNoticia.SoloLectura = false;
					this.previousNoticia.Accesibilidad = false;
				}
			},
			submitGetRequest(datos){
				this.currentId = datos.Id;
				this.noticia = datos; 	
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
			this.$on('sucess', function (msg) {
  				console.log(msg);})
		},
		updated(){
		/*	if(this.state == constantes.STATE_NEW){
				this.makeNewDetail();
			}*/
		}
	}
</script>

<style></style>