<template>
	<div id="plantillasMaster">
		<table class="table">
			<table class="table table-hover" role="tablist">
				<thead>
					<tr>
						<th>#</th>
						<th>Nombre</th>
						<th>Extensión</th>
						<th>Activo</th>
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
						<td>{{item.ExtCorreoE}}</td>
						<td>{{item.Activo}}</td>

					</tr>
					<tr  id="detail-tr" v-if="item.Id == elegido">
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
	import detail from './perfilesDetail.vue'
	export default{
		name: "Perfiles",
		components:{
			detail,
		},
		data (){
			return{
				lista: [],
				menuChoice:"PerfilesUsuarios",
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
		transition: all .3s ease;
		background-color: white;
	}	
#detail-tr.v-enter {
    animation: fadein .5s;
}

#detail-tr.v-leave {
    animation: fadeout .5s;
}


	

</style>