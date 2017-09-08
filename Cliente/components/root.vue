<template>
	<div id="Root" class="container-fluid">
		<div id = "Menu">
			<ul class="nav nav-tabs" id="myTab" role="tablist">
			  <li class="nav-item">
				<a id='Noticias'  data-toggle="tab"  class="nav-link active" href=""  role="tab"  @click="currentView='Noticias'" v-on:click.prevent="">Noticias</a>
			  </li>
			  <li class="nav-item">
				 <a id="Plantillas" class="nav-link" data-toggle="tab" href="" role="tab"  @click="currentView='Plantillas'" v-on:click.prevent="">Plantillas</a>
			  </li>
			  <li class="nav-item">
				<a id="Perfiles" class="nav-link" data-toggle="tab" href="" role="tab"  @click="currentView='Perfiles'" v-on:click.prevent=""> Perfiles de usuarios</a>
			  </li>
			</ul>

		</div>
		<hr>
		<div class="row">
			<component :is="currentView" class="col-12"></component>

		</div>
	</div>
</template>



<script>
	import Noticias from './noticiasMaster.vue'
	import detail from './detail.vue'

	import constantes from './constants.js'


	export default{
		components:{
			Noticias,
			detail
			
		},
		data (){
			return{
				currentView: 'Noticias'
			}
		},
		computed:{
			
		},
		methods:{
			anchorClass(tipo){

			}
		}
	}
</script>

<style></style>