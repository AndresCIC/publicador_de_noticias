<template>
	<div id="noticiasMaster">
		<table class="table">
			<table class="table table-hover" role="tablist">
				<thead>
					<tr>
						<th>#</th>
						<th>Titulo</th>
						<th>Tipo</th>
						<th>Fecha de publicación</th>
						<th> <i class="fa fa-user-plus" aria-hidden="true" v-on:click="getNewDetail()"></i> </a></th>
					</tr>
				</thead>			
				<tr  v-if="computeShowNewDetail">
					<td colspan="6">
						<noticiasDetail @cancelDetail ="removeDetail" @forceUpdate = "forceUpdate" :currentId = "elegido" :state ="state" role="tabpanel" class="float-right"> </noticiasDetail>
					</td>
				</tr>
				
				<tbody @click="" v-for="(noticia, index) in lista">

					<tr v-on:click="renderDetail(noticia.Id)">
						<th scope="row" v-model='index'>{{index}}</th>
						<td>{{noticia.Titulo}}</td>
						<td>{{noticia.Tipo}}</td>
						<td>{{noticia.FechaPublicacion}}</td>

					</tr>					

					<transition name="fade">
					<tr id="detail-tr" v-if="noticia.Id == elegido">
						<td colspan="6">
							<noticiasDetail  @makeGet= "recargarMaster" @forceUpdate = "forceUpdate" @cancelDetail = "removeDetail" :currentId = "elegido" :state = "state" role="tabpanel" class="float-right"> </noticiasDetail>
						</td>
					</tr>
					</transition>					
				</tbody>
				
			</table>
			
		</table>
	</div>
</template>
<script type="text/javascript">
	import constantes from './constants.js';
	import noticiasDetail from './noticiasDetail.vue';
	import { EventBus } from './eventBus.js';
	export default{
		name: "Noticias",
		components:{
			noticiasDetail,
		},
		data (){
			return{
				lista: [],
				menuChoice:"Noticias",
				state: "",
				elegido : "",
				messageType: 'alert-',
				message: '',
				showMessage : false,

			}
		},
		computed:{
			computeShowNewDetail(){
				return this.state === constantes.STATE_NEW;
			},
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
	/*			array.forEach(function(element, index) {
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
					if(element.SoloLectura){
						_this.lista[index].SoloLectura = "Si";
					}
					else{
						_this.lista[index].SoloLectura = "No";
					}
				});*/
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
		EventBus.$on('Message', (type, message) => {
			this.showMessage = true;
			this.messageType += type;
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