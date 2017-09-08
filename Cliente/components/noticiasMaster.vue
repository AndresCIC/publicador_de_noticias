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
						<detail @cancelDetail ="removeDetail" @forceUpdate = "forceUpdate" :currentId = "elegido" :state ="state" role="tabpanel" class="float-right"> </detail>
					</td>
				</tr>
				<tbody @click="" v-for="(noticia, index) in lista">

					<tr v-on:click="renderDetail(noticia.Id)">
						<th scope="row" v-model='index'>{{index}}</th>
						<td>{{noticia.Titulo}}</td>
						<td>{{noticia.Tipo}}</td>
						<td>{{noticia.FechaPublicacion}}</td>

					</tr>
					<tr id="detail-tr" v-if="noticia.Id == elegido">
						<td colspan="6">
							<detail @makeGet= "recargarMaster" @forceUpdate = "forceUpdate" @cancelDetail = "removeDetail" :currentId = "elegido" :state = "state" role="tabpanel" class="float-right"> </detail>
						</td>
					</tr>
					
				</tbody>

			</table>
			
		</table>
	</div>
</template>
<script type="text/javascript">
	import constantes from './constants.js';
	import detail from './noticiasDetail.vue'
	export default{
		name: "Noticias",
		components:{
			detail,
		},
		data (){
			return{
				lista: [],
				menuChoice:"Noticias",
				state: "",
				elegido : "",

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
				this.makeGetListRequest();
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
	},
}
</script>
<style type="text/css">
	#detail-tr:hover{
		background-color: white;
	}


</style>