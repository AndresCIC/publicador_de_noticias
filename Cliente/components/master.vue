<template>
	<div id="Master" class="master-div">		
		
		<table class="table table-hover">
			<thead>
				<tr>
					<th>#</th>
					<th>Título</th>
					<th>Última modificación</th>
					<th>Tipo</th>
					<th>Solo lectura</th>
							<th> <a href=""> <i class="fa fa-user-plus" aria-hidden="true" v-on:click="newDetail()"></i> </a></th>
						</tr>
					</thead>			 
					<tbody @click="" v-for="(item, index) in lista">

						<tr >

							<th scope="row" v-model='index'>{{index}}</th>
							<td>{{item.Titulo}}</td>
							<td>{{item.Titulo}}</td>
							<td>{{item.Tipo}}</td>
							<td>{{item.SoloLectura}}</td>		

						</tr>
					</tbody>			    
				</table>
			</div>
		</template>

<script>
import constantes from './constants.js';
import detail from './detail.vue';
	export default{
		components:{
		},
		data (){
			return{
				lista:{
				},
				menuChoice:"Documentos",
			}
		},
		computed:{

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
			submitGetListValues: function(datos){
				this.lista = datos;
			},
			emitEnableDetailEvent(read) {
		      // Send the event on a channel () with a payload ()
		      EventBus.$emit('enableDetail', this.read);
			},
			newDetail: function(index){
				this.read= false;
				this.emitEnableDetailEvent(this.read);
			},	
			readDetail: function(){
				
			},		
		},
		created(){
			this.makeGetListRequest();
		},
	}
</script>

<style></style>