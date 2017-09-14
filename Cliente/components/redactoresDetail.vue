<template>
	<div id="Detail-Redactores" class="container-fluid">
		<div id ="FormularioDocumentos" class="mx-auto" style="width: 45%">
			<center>
				<div class="form-group">
					<button :disabled="this.isEditable" id="enableEditButton" class="btn btn-primary" v-on:click="buttonEnableEdit">Modificar</button>
					<button  id="cancelarButton" class="btn btn-primary" v-on:click="buttonCancelar">Cancelar</button>
				</div>
			</center>

			<div class="form-group">
				<label>Nombre:</label>
				<input required :disabled="!isEditable" class="form-control" type="text" v-model="redactor.Nombre" id="NombreInput" placeholder="Introduzca título"></input>
			</div>
			<div class="form-group">
				<label>Apellidos:</label>
				<input required :disabled="!isEditable" class="form-control" type="text" v-model="redactor.Apellidos" id="ApellidosInput" placeholder="Introduzca título"></input>
			</div>

			<div class="form-group">
				<label>Rol:</label>
				<select required v-model="redactor.RolRedactor" class="form-control" :disabled="!isEditable">				
					<option value=1>Reportero</option>
					<option value=2>Redactor Gráfico</option>
					<option value=3>Ilustrador</option>
					<option value=4>Publicista</option>
				</select>
			</div>			

			<label>	<input :disabled="!isEditable" class="checkbox" type="checkbox" v-model="redactor.Disable" id="PublicarInput">Activado en sistema</label>
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
				    	<p slot="body"> ¿Está seguro que quiere borrar esta redactor? </p>
					    
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
				redactor:{
				},
				previousNoticia:{
				},
				isEditable:false,
				menuChoice : "Redactores",
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
					if(this.redactor.Nombre != this.previousNoticia.Nombre){
						return false;
					}
					else if(this.redactor.Apellidos != this.previousNoticia.Apellidos){
						return false;
					}
					else if(this.redactor.RolRedactor != this.previousNoticia.RolRedactoro){
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
				this.previousNoticia = $.extend({}, this.redactor)
			},
			buttonBorrar: function(){
				var self = this;
				EventBus.$once('Confirmation', confirm => {
					if(confirm === 'OK'){
						$.ajax({
								url:constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
								method: "DELETE"
							})
							.done(function() {self.borradoHandler()})
							.fail(function(){self.$emit('showCustomMessage','alert-danger', 'Se ha producido un error al borrar')})
						}
								
				});					
			},
			buttonCancelar: function(){
				this.$emit('cancelDetail', true);

			},
			borradoHandler: function(){								
				this.$emit('showCustomMessage', 'alert-success', 'Redactor borrado correctamente');
				this.$emit('makeGet', true);
				this.makeEmptyData();
			},
			buttonAccept: function(){
				if (this.state == constantes.STATE_UPDATE){
				// TODO: Se hace un PUT con el objeto

				$.ajax({url:constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
					method:"PUT",
					data: this.redactor})
				.done(this.putSubmitData);

				} 
				else
				{
					var self = this;
				$.ajax({url:constantes.BASE_URL + this.menuChoice,
					method:"POST",
					data: this.redactor})	
				.done(function() {self.afterPostHandler()})
				.fail(function(){
					self.$emit('showCustomMessage','alert-error', 'Fallo al crear el elemento');
				})
			}			

			},
			afterPostHandler: function(){
				this.$emit('showCustomMessage','alert-success', 'Elemento creado correctamente');
				this.$emit('forceUpdate', true);

				// TODO: Se fuerza un get en el maestro y se cierra el detail.
				// Podemos llamar al metodo buttonCancelar.
			},
			putSubmitData(){
				alert("Elemento modificado");
				this.previousNoticia = $.extend({}, this.redactor);
				this.$emit('forceUpdate', true);

			}, 	
			makeGetRequest(){
				var self = this;
				$.ajax({
					url: constantes.BASE_URL + this.menuChoice + "/" + this.currentId,
					method: "GET"
				})
				.done(this.submitGetRequest)
				.fail(function(){
					self.$emit('showCustomMessage', 'alert-danger', 'Se ha producido un fallo al cargar el redactor');
				})
			},
			makeEmptyData(){
				if(!this.estaVacio){
					this.redactor = {};
					this.currentId = "";
					this.redactor.Nombre = "";
					this.redactor.Apellidos = "";
					this.redactor.RolRedactor = "";
					this.redactor.Disable = false;


					this.previousNoticia={};
					this.previousNoticia.Nombre = "";
					this.previousNoticia.Apellidos = "";
					this.previousNoticia.RolRedactor = "";
					this.previousNoticia.Disable = false;
				}
			},
			submitGetRequest(datos){
				this.currentId = datos.Id;
				this.redactor = datos; 	
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