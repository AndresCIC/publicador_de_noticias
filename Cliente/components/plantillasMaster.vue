<template>
	<div id="plantillasMaster">
		<table class="table">
			<table class="table table-hover" role="tablist">
				<thead>
					<tr>
						<th>#</th>
						<th>Nombre</th>
						<th>Estilo</th>
						<th>Precio</th>
						<th> <i class="fa fa-user-plus" aria-hidden="true" v-on:click="getNewDetail()"></i> </a></th>
					</tr>
				</thead>			
				<tr  v-if="computeShowNewDetail">
					<td colspan="6">
						<detail @cancelDetail ="removeDetail" @forceUpdate = "forceUpdate" :currentId = "elegido" :state ="state" role="tabpanel" class="float-right"> </detail>
					</td>
				</tr>
				<tbody @click="" v-for="(item, index) in lista">

					<tr v-on:click="renderDetail(item.Id)">
						<th scope="row" v-model='index'>{{index}}</th>
						<td>{{item.Nombre}}</td>
						<td>{{item.Estilo}}</td>
						<td>{{item.Precio}}</td>

					</tr>
					<tr id="detail-tr" v-if="item.Id == elegido">
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
	import detail from './detailPlantillas.vue'
	export default{
		name: "Plantillas",
		components:{
			detail,
		},
		data (){
			return{
				lista: [],
				menuChoice:"Plantillas",
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
				array.forEach(function(element, index) {
					if(element.Estilo == 1){
						_this.lista[index].Estilo = "Formal";
					}
					else if(element.Estilo == 2){
						_this.lista[index].Estilo = "Informal";
					}
					else if(element.Estilo == 3){
						_this.lista[index].Estilo = "Libro";
					}
					else if(element.Estilo == 4){
						_this.lista[index].Estilo = "Académico";
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
	},
}
</script>
<style type="text/css">
	#detail-tr:hover{
		background-color: white;
	}	
</style>