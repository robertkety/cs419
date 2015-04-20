from django.contrib import admin

from .models import organization, item, repairable, reusable, category, item_category

admin.site.register(organization)
admin.site.register(item)
admin.site.register(repairable)
admin.site.register(reusable)
admin.site.register(category)
admin.site.register(item_category)