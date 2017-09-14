<template>
	<div id="redactoresMaster">

			<table class="table table-hover" role="tablist">
				<thead>
					<tr>
						<th>#</th>
						<th>Nombre</th>
						<th>Apellidos</th>
						<th>Rol</th>
						<th> <i class="fa fa-user-plus" aria-hidden="true" v-on:click="getNewDetail()"></i> </a></th>
					</tr>
				</thead>			
				<tr v-if="computeShowNewDetail">
					<td colspan="6">
						<redactoresDetail @cancelDetail ="removeDetail" @forceUpdate = "forceUpdate" :currentId = "elegido" :state ="state" role="tabpanel" class="float-right"> </redactoresDetail>
					</td>
				</tr>					
				<tbody @click="" v-for="(redactor, index) in lista">

					<tr v-on:click="renderDetail(redactor.Id)">
						<th scope="row" v-model='index'>{{index}}</th>
						<td>{{redactor.Nombre}}</td>
						<td>{{redactor.Apellidos}}</td>
						<td>{{redactor.RolRedactor}}</td>

					</tr>					

					<transition name="fade">
					<tr id="detail-tr" v-if="redactor.Id == elegido">
						<td colspan="6">
							<redactoresDetail  @makeGet= "recargarMaster" @showCustomMessage="showCustomMessage" @forceUpdate = "forceUpdate" @cancelDetail = "removeDetail" :currentId = "elegido" :state = "state" role="tabpanel" class="float-right"> </redactoresDetail>
						</td>						
					</tr>
					
				</transition>
				
										
				</tbody>
						
			</table>

			<modal v-if="showMessage">
			    <!--
			      you can use custom content here to overwrite
			      default content
			    -->
			    <i slot="header"/>
		    	<p slot="body"> 
		    		<customMessage :message="message" :messageType="messageType" @closeMessage="showMessage = false" >					
					</customMessage> 
				</p>
				<p slot="buttonAccept"></p>
				<p slot="buttonCancel"></p>

			    
			  </modal>

	</div>
</template>
<script type="text/javascript">
	import constantes from './constants.js';
	import redactoresDetail from './redactoresDetail.vue';	
	import customMessage from './customMessage.vue';
	import { EventBus } from './eventBus.js';
	import modal  from './modal.vue';
	export default{
		name: "Redactores_Master",		
		components:{
			redactoresDetail,
			customMessage,
			modal,
		},
		data (){
			return{
				lista: [],
				menuChoice:"Redactores",
				state: "",
				elegido : "",
				messageType: 'alert-',
				showMessage : false,
				message : '',

			}
		},
		computed:{
			computeShowNewDetail(){
				return this.state === constantes.STATE_NEW;
			},
			computedMessage(){
				var _mutableMessage = this.message;
				return _mutableMessage;
			}
		},
		methods:{
			makeGetListRequest(){
				$.ajax({
					url: constantes.BASE_URL + this.menuChoice,
					method: "GET"
				})
				.done(this.submitGetListValues)
				.fail(function(){
					alert("Ha fallado la carga del objeto");
				})
			},
			forceUpdate: function(){
				this.removeDetail();
				this.makeGetListRequest();
			},
			showCustomMessage: function(type,message){				
				this.messageType = type;
				this.message = message;
				this.showMessage = true;
			},
			recargarMaster: function(){
				this.removeDetail();
				this.makeGetListRequest()
			},
			removeDetail: function(){
				this.elegido = "";
				this.state = "";
			},
			submitGetListValues: function(datos){
				this.lista = datos;
				this.parseTipo(datos);
			},
			parseTipo: function(array){
				var _this = this;
				array.forEach(function(element, index) {
					if(element.RolRedactor == 4){
						_this.lista[index].RolRedactor = "Publicista";
					}
					else if(element.RolRedactor == 1){
						_this.lista[index].RolRedactor = "Reportero";
					}
					else if(element.RolRedactor == 2){
						_this.lista[index].RolRedactor = "Redactor Gráfico";
					}
					else if(element.RolRedactor == 3){
						_this.lista[index].RolRedactor = "Ilustrador";
					}
				});
			},

			emitEnableDetailEvent(read) {
		      // Send the event on a channel () with a payload ()
		      EventBus.$emit('enableDetail', this.read);
		  },
		  getNewDetail: function(){
		  	this.state = constantes.STATE_NEW;
		  	this.elegido = "";
				//this.emitEnableDetailEvent(this.read);
			},	
			renderDetail: function(index){
				if(this.state == constantes.STATE_UPDATE){
					if(this.elegido == index){
						this.elegido ="";
						this.state = "";
					}
					else {
						this.elegido = index;
					}
				}
				else{
					this.state = constantes.STATE_UPDATE
					this.elegido = index;
				//this.showDetail == false ? this.showDetail = true :  this.showDetail = false;
			}

		},		
	},
	created(){
		this.makeGetListRequest();
		EventBus.$on('showCustomMessage', (type, message) => {
			this.showMessage = true;
			this.messageType = type;
			this.message = message;
		});
	},
}
</script>
<style type="text/css" scoped>
	#detail-tr:hover{
		background-color: white;
	};

.fade-enter-active, .fade-leave-active {
   	opacity: 1;
   	transition: height 300ms cubic-bezier(0.17, 0.04, 0.03, 0.94);
    transition: 
	
};
.fade-enter, .fade-leave-to  {
  opacity: 0;
};

</style>