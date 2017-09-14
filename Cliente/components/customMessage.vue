<template>
	<transition name="MessageTransition" >
		<div class="alert"  v-bind:class="messageType">
			{{message}}				
		</div>
	</transition>
</template>
<script type="text/javascript">
export default{

	name: "Noticias",
	props: ['message', 'messageType'],
		data (){
			return{
			}
		},
		methods: {

	},
	created(){
		var self = this;
		setTimeout(function(){
		  self.$emit('closeMessage');
		}, 1200);		
	},
}



</script>
<style type="text/css">
	

</style>