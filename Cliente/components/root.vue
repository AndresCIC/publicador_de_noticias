<template>
	<div id="Root" class="container-fluid">
		<div id = "Menu">
			<ul class="nav nav-tabs" id="myTab" role="tablist">
			  <li class="nav-item">
				<a id='Noticias'  data-toggle="tab"  class="nav-link active" href=""  role="tab"  @click="currentView='Noticias'" v-on:click.prevent="">Noticias</a>
			  </li>
			  <li class="nav-item">
				 <a id="Redactores" class="nav-link" data-toggle="tab" href="" role="tab"  @click="currentView='Redactores'" v-on:click.prevent="">Redactores</a>
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
	import Redactores from './redactoresMaster.vue'

	import constantes from './constants.js'


	export default{
		components:{
			Noticias,
			Redactores,
			
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